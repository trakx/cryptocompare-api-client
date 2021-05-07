using System;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Trakx.WebSockets
{
    public class WebSocketAdapter : IWebSocketAdapter
    {

        private readonly ILogger _logger = Log.Logger.ForContext(MethodBase.GetCurrentMethod()!.DeclaringType);
        private ClientWebSocket _client = new();
        private Uri _uri;

        #region Implementation of IClientWebsocket

        /// <inheritdoc />
        public WebSocketCloseStatus? CloseStatus => _client.CloseStatus;

        /// <inheritdoc />
        public string CloseStatusDescription => _client.CloseStatusDescription;

        /// <inheritdoc />
        public ClientWebSocketOptions Options => _client.Options;

        /// <inheritdoc />
        public WebSocketState State => _client.State;

        /// <inheritdoc />
        public string SubProtocol => _client.SubProtocol;

        /// <inheritdoc />
        public void Abort()
        {
            _client.Abort();
        }

        /// <inheritdoc />
        public async Task CloseAsync(WebSocketCloseStatus closeStatus,
            string statusDescription, CancellationToken cancellationToken)
        {
            if(_client.State == WebSocketState.Closed || _client.State == WebSocketState.Aborted) return;
            await _client.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        /// <inheritdoc />
        public async Task CloseOutputAsync(WebSocketCloseStatus closeStatus,
            string statusDescription, CancellationToken cancellationToken)
        {
            await _client.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
        }

        /// <inheritdoc />
        public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            _uri = uri;
            await _client.ConnectAsync(uri, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer,
            CancellationToken cancellationToken)
        {
            return await _client.ReceiveAsync(buffer, cancellationToken);
        }

        /// <inheritdoc />
        public async ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer,
            CancellationToken cancellationToken)
        {
            return await _client.ReceiveAsync(buffer, cancellationToken);
        }

        /// <inheritdoc />
        public async Task SendAsync(ArraySegment<byte> buffer,
            WebSocketMessageType messageType, bool endOfMessage,
            CancellationToken cancellationToken)
        {
            await _client.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        /// <inheritdoc />
        public async ValueTask SendAsync(ReadOnlyMemory<byte> buffer,
            WebSocketMessageType messageType, bool endOfMessage,
            CancellationToken cancellationToken)
        {
            await _client.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        public async Task PingServer(string message, CancellationToken cancellationToken)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message).AsMemory();
            await SendAsync(messageBytes, WebSocketMessageType.Text, true,
                cancellationToken);
        }

        public async Task<bool> RecycleConnectionAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Attempting to reconnect to '{_uri}'...");
                _client.Dispose();
                _client = new ClientWebSocket();
                await _client.ConnectAsync(_uri!, cancellationToken).ConfigureAwait(false);
                return _client.State == WebSocketState.Open;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            _client.Dispose();
        }

        #endregion
    }
}
