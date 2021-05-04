using System;
using System.Net.WebSockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Serilog;

namespace Trakx.CryptoCompare.ApiClient.WebSocket
{
    /// <summary>
    /// Simple wrapper around native <see cref="System.Net.WebSockets.ClientWebSocket" /> to allow
    /// a bit of unit testing.
    /// </summary>
    public interface IClientWebsocket : IDisposable
    {
        /// <summary>Gets the reason why the close handshake was initiated on <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
        /// <returns>The reason why the close handshake was initiated.</returns>
        WebSocketCloseStatus? CloseStatus { get; }
        /// <summary>Gets a description of the reason why the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance was closed.</summary>
        /// <returns>The description of the reason why the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance was closed.</returns>
        string CloseStatusDescription { get; }
        /// <summary>Gets the WebSocket options for the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
        /// <returns>The WebSocket options for the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</returns>
        ClientWebSocketOptions Options { get; }
        /// <summary>Gets the WebSocket state of the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
        /// <returns>The WebSocket state of the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</returns>
        WebSocketState State { get; }
        /// <summary>Gets the supported WebSocket sub-protocol for the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
        /// <returns>The supported WebSocket sub-protocol.</returns>
        string SubProtocol { get; }
        /// <summary>Aborts the connection and cancels any pending IO operations.</summary>
        void Abort();
        /// <summary>Close the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance as an asynchronous operation.</summary>
        /// <param name="closeStatus">The WebSocket close status.</param>
        /// <param name="statusDescription">A description of the close status.</param>
        /// <param name="cancellationToken">A cancellation token used to propagate notification that this  operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task CloseAsync(
          WebSocketCloseStatus closeStatus,
          string statusDescription,
          CancellationToken cancellationToken);
        /// <summary>Close the output for the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance as an asynchronous operation.</summary>
        /// <param name="closeStatus">The WebSocket close status.</param>
        /// <param name="statusDescription">A description of the close status.</param>
        /// <param name="cancellationToken">A cancellation token used to propagate notification that this  operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task CloseOutputAsync(
          WebSocketCloseStatus closeStatus,
          string statusDescription,
          CancellationToken cancellationToken);
        /// <summary>Connect to a WebSocket server as an asynchronous operation.</summary>
        /// <param name="uri">The URI of the WebSocket server to connect to.</param>
        /// <param name="cancellationToken">A cancellation token used to propagate notification that the  operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task ConnectAsync(Uri uri, CancellationToken cancellationToken);

        /// <summary>Receives data on <see cref="T:System.Net.WebSockets.ClientWebSocket" /> as an asynchronous operation.</summary>
        /// <param name="buffer">The buffer to receive the response.</param>
        /// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.WebSockets.ClientWebSocket" /> is not connected.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.WebSockets.ClientWebSocket" /> has been closed.</exception>
        Task<WebSocketReceiveResult> ReceiveAsync(
          ArraySegment<byte> buffer,
          CancellationToken cancellationToken);
        /// <summary>Receives data on <see cref="T:System.Net.WebSockets.ClientWebSocket" /> to a byte memory range as an asynchronous operation.</summary>
        /// <param name="buffer">The region of memory to receive the response.</param>
        /// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.WebSockets.ClientWebSocket" /> is not connected.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.WebSockets.ClientWebSocket" /> has been closed.</exception>
        ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(
          Memory<byte> buffer,
          CancellationToken cancellationToken);
        /// <summary>Sends data on <see cref="T:System.Net.WebSockets.ClientWebSocket" /> as an asynchronous operation.</summary>
        /// <param name="buffer">The buffer containing the message to be sent.</param>
        /// <param name="messageType">One of the enumeration values that specifies whether the buffer is clear text or in a binary format.</param>
        /// <param name="endOfMessage">
        /// <see langword="true" /> to indicate this is the final asynchronous send; otherwise, <see langword="false" />.</param>
        /// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.WebSockets.ClientWebSocket" /> is not connected.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.WebSockets.ClientWebSocket" /> has been closed.</exception>
        Task SendAsync(
          ArraySegment<byte> buffer,
          WebSocketMessageType messageType,
          bool endOfMessage,
          CancellationToken cancellationToken);
        /// <summary>Sends data on <see cref="T:System.Net.WebSockets.ClientWebSocket" /> from a read-only byte memory range as an asynchronous operation.</summary>
        /// <param name="buffer">The region of memory containing the message to be sent.</param>
        /// <param name="messageType">One of the enumeration values that specifies whether the buffer is clear text or in a binary format.</param>
        /// <param name="endOfMessage">
        /// <see langword="true" /> to indicate this is the final asynchronous send; otherwise, <see langword="false" />.</param>
        /// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.WebSockets.ClientWebSocket" /> is not connected.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.WebSockets.ClientWebSocket" /> has been closed.</exception>
        ValueTask SendAsync(
          ReadOnlyMemory<byte> buffer,
          WebSocketMessageType messageType,
          bool endOfMessage,
          CancellationToken cancellationToken);

    }

    public sealed class ResilientClientWebsocket : IClientWebsocket
    {
        private ClientWebSocket _client;
        private Uri _uri;
        private CancellationToken _cancellationToken;
        private static readonly ILogger Logger = Log.Logger.ForContext(MethodBase.GetCurrentMethod()!.DeclaringType);

        public ResilientClientWebsocket()
        {
            _client = new();
        }

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            _client.Dispose();
        }

        #endregion

        #region Implementation of IClientWebsocket

        /// <inheritdoc />
        public WebSocketCloseStatus? CloseStatus => _client.CloseStatus;

        /// <inheritdoc />
        public string? CloseStatusDescription => _client.CloseStatusDescription;

        /// <inheritdoc />
        public ClientWebSocketOptions Options => _client.Options;

        /// <inheritdoc />
        public WebSocketState State => _client.State;

        /// <inheritdoc />
        public string? SubProtocol => _client.SubProtocol;

        /// <inheritdoc />
        public void Abort()
        {
            _client.Abort();
        }

        /// <inheritdoc />
        public async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            await _client.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        /// <inheritdoc />
        public async Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            await _client.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
        }

        /// <inheritdoc />
        public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            _uri = uri;
            _cancellationToken = cancellationToken;
            await _client.ConnectAsync(uri, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer,
            CancellationToken cancellationToken)
        {
            var result = await ReceiveAsyncWithRetry(async _
                => await _client.ReceiveAsync(buffer, cancellationToken)
                    .ConfigureAwait(false), cancellationToken);
            return result!;
        }

        /// <inheritdoc />
        public async ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer,
            CancellationToken cancellationToken)
        {
            return await ReceiveAsyncWithRetry(async _
                => await _client.ReceiveAsync(buffer, cancellationToken)
                    .ConfigureAwait(false), cancellationToken);
        }

        /// <inheritdoc />
        public async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage,
            CancellationToken cancellationToken)
        {
            await _client.SendAsync(buffer, messageType, endOfMessage, cancellationToken);

        }

        /// <inheritdoc />
        public async ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage,
            CancellationToken cancellationToken)
        {
            await _client.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        #endregion

        #region Helper Methods

        private async ValueTask<TResult?> ReceiveAsyncWithRetry<TResult>(Func<CancellationToken, Task<TResult>> func,
            CancellationToken cancellationToken)
        {
            var retry = Policy.Handle<Exception>()
                .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3),
                    (_, _) => TryReconnect().ConfigureAwait(false));
            return await retry.ExecuteAsync(async () =>
            {
                if (_cancellationToken.IsCancellationRequested) return default;
                return await func(cancellationToken).ConfigureAwait(false);
            });
        }

        private async Task TryReconnect()
        {
            Logger.Information($"Attempting to reconnect to '{_uri}'...");
            try
            {
                _client.Dispose();
                _client = new ClientWebSocket();
                await _client.ConnectAsync(_uri, _cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                // in this context, issues should be ignored.
            }
        }

        #endregion

    }
}