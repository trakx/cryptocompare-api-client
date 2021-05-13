using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Trakx.WebSockets
{
    /// <summary>
    /// Simple wrapper around native <see cref="System.Net.WebSockets.ClientWebSocket" /> to allow
    /// a bit of unit testing.
    /// </summary>
    public interface IWebSocketAdapter : IDisposable
    {
        /// <summary>Gets the reason why the close handshake was initiated on <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
        /// <returns>The reason why the close handshake was initiated.</returns>
        WebSocketCloseStatus? CloseStatus { get; }
        /// <summary>Gets a description of the reason why the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance was closed.</summary>
        /// <returns>The description of the reason why the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance was closed.</returns>
        string? CloseStatusDescription { get; }
        /// <summary>Gets the WebSocket options for the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
        /// <returns>The WebSocket options for the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</returns>
        ClientWebSocketOptions Options { get; }
        /// <summary>Gets the WebSocket state of the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
        /// <returns>The WebSocket state of the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</returns>
        WebSocketState State { get; }
        /// <summary>Gets the supported WebSocket sub-protocol for the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
        /// <returns>The supported WebSocket sub-protocol.</returns>
        string? SubProtocol { get; }
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
        /// <param name="uri">uri to connect to the websocket server.</param>
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
        /// 
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
        /// 
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

        /// <summary>
        /// Send a message to the server represents a ping.
        /// </summary>
        /// <param name="message">Message to be sent to the server</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task PingServer(string message, CancellationToken cancellationToken);

        /// <summary>
        /// Recycle the connection with the dotnet websocket. It reconnects the user the connection gets lost.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> RecycleConnectionAsync(CancellationToken cancellationToken);

    }

}
