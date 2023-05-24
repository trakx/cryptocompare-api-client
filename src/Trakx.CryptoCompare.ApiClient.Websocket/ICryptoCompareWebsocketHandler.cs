using System.Threading.Tasks;
using Trakx.CryptoCompare.ApiClient.Websocket.Model;
using Trakx.Websocket.Interfaces;
using Trakx.Websocket.Model;

namespace Trakx.CryptoCompare.ApiClient.Websocket;

public interface ICryptoCompareWebsocketHandler : IClientWebsocketRedirectHandler<InboundMessageBase>
{
    /// <summary>Removes the specified session subscription.</summary>
    /// <param name="subscription">The subscription.</param>
    Task<bool> RemoveAsync(TopicSubscription subscription);
}
