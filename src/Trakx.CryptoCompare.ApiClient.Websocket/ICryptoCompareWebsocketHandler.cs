using Trakx.CryptoCompare.ApiClient.Websocket.Model;
using Trakx.Websocket.Interfaces;

namespace Trakx.CryptoCompare.ApiClient.Websocket;

public interface ICryptoCompareWebsocketHandler : IClientWebsocketRedirectHandler<InboundMessageBase>
{
}
