namespace Trakx.WebSockets.KeepAlivePolicies
{
    public interface IKeepAlivePolicy
    {

        bool TryReconnectWhenExceptionHappens { get; }

        void ApplyStrategy<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage
            where TStreamer : IWebSocketStreamer<TInboundMessage>;

    }
}
