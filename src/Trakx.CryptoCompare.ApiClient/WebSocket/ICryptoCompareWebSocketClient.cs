using System.Threading.Tasks;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound;
using Trakx.WebSockets;

namespace Trakx.CryptoCompare.ApiClient.WebSocket
{
    public interface ICryptoCompareWebSocketClient : IWebSocketClient<InboundMessageBase, ICryptoCompareWebSocketStreamer>
    {

        Task AddSubscriptions(params ICryptoCompareSubscription[] subscriptions);

        Task RemoveSubscriptions(params ICryptoCompareSubscription[] subscriptions);

    }
}