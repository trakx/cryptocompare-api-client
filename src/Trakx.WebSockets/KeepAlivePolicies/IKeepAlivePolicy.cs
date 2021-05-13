namespace Trakx.WebSockets.KeepAlivePolicies
{
    public interface IKeepAlivePolicy
    {

        bool TryReconnectWhenWebSocketErrors { get; }

        void Apply<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage
            where TStreamer : IWebSocketStreamer<TInboundMessage>;

    }
}
