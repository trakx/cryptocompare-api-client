using Trakx.WebSockets.KeepAlivePolicies;
using Trakx.WebSockets.Tests.Unit.DummyClient.Messages;

namespace Trakx.WebSockets.Tests.Unit.DummyClient
{
    public class DummyWebSocketClient : WebSocketClient<BaseInboundMessage, IDummyWebSocketStreamer>
    {
        public DummyWebSocketClient(IWebSocketAdapter websocket, string baseUrl, 
            IKeepAlivePolicy keepAlivePolicy, IDummyWebSocketStreamer streamer) 
            : base(websocket, baseUrl, keepAlivePolicy, streamer)
        {
        }
    }
}
