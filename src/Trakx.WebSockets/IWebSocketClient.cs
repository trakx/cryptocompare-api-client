using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Trakx.WebSockets
{

    /// <summary>
    /// Base interface to be implemented by the clients of Trakx.WebSockets.
    /// </summary>
    /// <typeparam name="TBaseMessage"></typeparam>
    /// <typeparam name="TStreamer"></typeparam>
    public interface IWebSocketClient<TBaseMessage, out TStreamer>
    where TStreamer : IWebSocketStreamer<TBaseMessage>
    {

        /// <summary>
        /// This property gives access to the inner websocket connection being used.
        /// </summary>
        IWebSocketAdapter WebSocket { get; }

        /// <summary>
        /// This property gives access to the provider of streams.
        /// </summary>
        TStreamer Streamer { get; }

        /// <summary>
        /// It returns the status of the last task executed by the listener.
        /// </summary>
        TaskStatus? ListenInboundMessagesTaskStatus { get; }

    }
}
