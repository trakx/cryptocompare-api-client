namespace Trakx.WebSockets.Tests.Unit.DummyClient.Messages
{
    public abstract class BaseInboundMessage : IBaseInboundMessage
    {
        protected BaseInboundMessage(string type)
        {
            Type = type;
        }

        public string Type { get; }
    }
}
