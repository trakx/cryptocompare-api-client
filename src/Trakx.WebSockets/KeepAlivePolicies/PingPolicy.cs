namespace Trakx.WebSockets.KeepAlivePolicies
{
    public class PingPolicy : IKeepAlivePolicy
    {

        public bool TryReconnectWhenExceptionHappens => true;

        public void ApplyStrategy<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client) 
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
        }

    }
}
