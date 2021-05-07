using System;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Trakx.WebSockets.KeepAlivePolicies;

namespace Trakx.WebSockets
{
    public abstract class WebSocketClient<TBaseMessage, TStreamer> : IWebSocketClient<TBaseMessage, TStreamer>, IAsyncDisposable
        where TBaseMessage : IBaseInboundMessage
        where TStreamer : IWebSocketStreamer<TBaseMessage>
    {

        private readonly ILogger _logger = Log.Logger.ForContext(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IKeepAlivePolicy _keepAlivePolicy;
        private readonly string _baseUrl;
        private Task? _listenToWebSocketTask;
        private readonly CancellationTokenSource _cancellationTokenSource;


        protected WebSocketClient(IWebSocketAdapter websocket, string baseUrl,
            IKeepAlivePolicy keepAlivePolicy, TStreamer streamer)
        {
            WebSocket = websocket;
            Streamer = streamer;
            _baseUrl = baseUrl;
            _keepAlivePolicy = keepAlivePolicy;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public TaskStatus? ListenInboundMessagesTaskStatus => _listenToWebSocketTask?.Status;

        public async Task Connect()
        {
            _logger.Information("Opening Wrapped websocket client");
            if (WebSocket.State != WebSocketState.Open)
                await WebSocket.ConnectAsync(new Uri(_baseUrl), _cancellationTokenSource.Token).ConfigureAwait(false);
            _logger.Information("Wrapped websocket state {0}", WebSocket.State);
            await StartListening(_cancellationTokenSource.Token).ConfigureAwait(false);
            _keepAlivePolicy.ApplyStrategy(this);
        }

        private async Task StartListening(CancellationToken cancellationToken)
        {
            _listenToWebSocketTask = await Task.Factory.StartNew(async () =>
            {
                while (WebSocket.State == WebSocketState.Open && !_cancellationTokenSource.IsCancellationRequested)
                {
                    var buffer = new ArraySegment<byte>(new byte[4096]);
                    var receiveResult = await ReceiveAsyncWithStrategy(async () => await WebSocket.ReceiveAsync(buffer, cancellationToken)).ConfigureAwait(false);
                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;
                    var msgBytes = buffer.Skip(buffer.Offset).Take(receiveResult.Count).ToArray();
                    var result = Encoding.UTF8.GetString(msgBytes);

                    if (!string.IsNullOrWhiteSpace(result)) Streamer.PublishInboundMessageOnStream(result);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default).ConfigureAwait(false);
            _logger.Information("Listening to incoming messages");
        }

        private async Task<WebSocketReceiveResult> ReceiveAsyncWithStrategy(Func<Task<WebSocketReceiveResult>> func)
        {
            while (true)
            {
                try
                {
                    return await func().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Failed to receive data from {_baseUrl}");
                    if (!_keepAlivePolicy.TryReconnectWhenExceptionHappens) throw;
                    else await TryReconnect().ConfigureAwait(false);
                }
            }
        }

        public async Task Disconnect()
        {
            _logger.Information("Closing CryptoCompare websocket");
            await WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                "CryptoCompare WebClient getting disposed.",
                _cancellationTokenSource.Token);
            _logger.Information("Closed CryptoCompare websocket");
        }

        private async Task StopListening()
        {
            while (_listenToWebSocketTask != null && _listenToWebSocketTask.Status < TaskStatus.RanToCompletion)
            {
                await Task.Delay(100, _cancellationTokenSource.Token).ConfigureAwait(false);
            }
            _logger.Information("CryptoCompare websocket state {0}", WebSocket.State);
        }

        public IWebSocketAdapter WebSocket { get; }

        public TStreamer Streamer { get; }

        #region Helper Methods

        private async Task TryReconnect()
        {
            await WebSocket.RecycleConnectionAsync(_cancellationTokenSource.Token);
        }

        #endregion

        #region IDisposable

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!disposing) return;
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
            }
            await Disconnect().ConfigureAwait(false);
            await StopListening().ConfigureAwait(false);

            _cancellationTokenSource?.Dispose();
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            DisposeAsync(disposing).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            WebSocket.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
