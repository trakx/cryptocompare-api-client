namespace Trakx.WebSockets.KeepAlivePolicies
{
    public class NoPolicy : IKeepAlivePolicy
    {

        public bool TryReconnectWhenWebSocketErrors => false;
        public void Apply<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            // No behavior should be implemented for NoPolicy class
        }
    }
}
