namespace Trakx.WebSockets.KeepAlivePolicies
{
    public class NoPolicy : IKeepAlivePolicy
    {

        public bool TryReconnectWhenExceptionHappens => false;
        public void ApplyStrategy<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
        }
    }
}
