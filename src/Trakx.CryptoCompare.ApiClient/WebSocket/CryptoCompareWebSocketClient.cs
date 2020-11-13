using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Outbound;

namespace Trakx.CryptoCompare.ApiClient.WebSocket
{
    public class CryptoCompareWebSocketClient : IAsyncDisposable, ICryptoCompareWebSocketClient, IDisposable
    {
        private readonly IApiDetailsProvider _apiDetailsProvider;
        private readonly IClientWebsocket _client;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly ILogger _logger;
        private Task? _listenToWebSocketTask;

        public IWebSocketStreamer WebSocketStreamer { get; }

        public CryptoCompareWebSocketClient(IClientWebsocket clientWebSocket,
            IApiDetailsProvider apiDetailsProvider, 
            IWebSocketStreamer webSocketStreamer, 
            ILogger logger)
        {
            _apiDetailsProvider = apiDetailsProvider;
            _client = clientWebSocket;
            WebSocketStreamer = webSocketStreamer;
            _logger = logger;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task Connect()
        {
            _logger.Information("Opening CryptoCompare websocket");
            if (_client.State != WebSocketState.Open) 
                await _client.ConnectAsync(_apiDetailsProvider.WebSocketEndpoint, _cancellationTokenSource.Token).ConfigureAwait(false);
            _logger.Information("CryptoCompare websocket state {0}", State);
            await StartListening(_cancellationTokenSource.Token).ConfigureAwait(false);
        }

        public WebSocketState State => _client.State;
        public TaskStatus? ListenInboundMessagesTaskStatus => _listenToWebSocketTask?.Status;

        public async Task AddSubscriptions(params ICryptoCompareSubscription[] subscriptions)
        {
            var message = new AddSubscriptionMessage(subscriptions);
            var serialize = JsonSerializer.Serialize(message);
            try
            {
                await _client.SendAsync(Encoding.UTF8.GetBytes(serialize),
                    WebSocketMessageType.Text, true, _cancellationTokenSource.Token);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Failed to add subscriptions.");
            }
        }

        public async Task RemoveSubscriptions(params ICryptoCompareSubscription[] subscriptions)
        {
            var message = new RemoveSubscriptionMessage(subscriptions);
            await _client.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)),
                WebSocketMessageType.Text, true, _cancellationTokenSource.Token);
        }

        private async Task StartListening(CancellationToken cancellationToken)
        {
            _listenToWebSocketTask = await Task.Factory.StartNew(async () =>
            {
                while (_client.State == WebSocketState.Open && !_cancellationTokenSource.IsCancellationRequested)
                {
                    var buffer = new ArraySegment<byte>(new byte[4096]);
                    var receiveResult = await _client.ReceiveAsync(buffer, cancellationToken).ConfigureAwait(false);
                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;
                    var msgBytes = buffer.Skip(buffer.Offset).Take(receiveResult.Count).ToArray();
                    var result = Encoding.UTF8.GetString(msgBytes);

                    if (!string.IsNullOrWhiteSpace(result)) WebSocketStreamer.PublishInboundMessageOnStream(result);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default).ConfigureAwait(false);
            _logger.Information("Listening to incoming messages");   
        }

        public async Task Disconnect()
        {
            _logger.Information("Closing CryptoCompare websocket");
            await _client.CloseAsync(WebSocketCloseStatus.NormalClosure,
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
            _logger.Information("CryptoCompare websocket state {0}", State);
        }

        #region IDisposable

        protected virtual async ValueTask DisposeAsync(bool disposing)
        { 
            if (!disposing) return;
            _cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
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

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
