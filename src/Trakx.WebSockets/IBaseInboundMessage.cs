namespace Trakx.WebSockets
{

    /// <summary>
    /// The basic inbound message in which the other messages derive from.
    /// </summary>
    public interface IBaseInboundMessage
    {
        string Type { get; }

    }
}
