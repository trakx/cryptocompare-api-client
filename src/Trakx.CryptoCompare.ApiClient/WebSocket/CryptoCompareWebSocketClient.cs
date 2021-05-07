using System;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Serilog;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Outbound;
using Trakx.WebSockets;
using Trakx.WebSockets.KeepAlivePolicies;

namespace Trakx.CryptoCompare.ApiClient.WebSocket
{
    public class CryptoCompareWebSocketClient : WebSocketClient<InboundMessageBase, ICryptoCompareWebSocketStreamer>, ICryptoCompareWebSocketClient
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        private static readonly ILogger Logger = Log.Logger.ForContext(MethodBase.GetCurrentMethod()!.DeclaringType);

        public CryptoCompareWebSocketClient(IWebSocketAdapter webSocket,
            IKeepAlivePolicy strategy, ICryptoCompareWebSocketStreamer streamer,
            IOptions<CryptoCompareApiConfiguration> configuration)
            : base(webSocket, configuration.Value.WebSocketEndpoint.ToString(), strategy, streamer)
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task AddSubscriptions(params ICryptoCompareSubscription[] subscriptions)
        {
            var message = new AddSubscriptionMessage(subscriptions);
            var serialize = JsonSerializer.Serialize(message);
            try
            {
                await WebSocket.SendAsync(Encoding.UTF8.GetBytes(serialize),
                    WebSocketMessageType.Text, true, _cancellationTokenSource.Token);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Failed to add subscriptions.");
            }
        }

        public async Task RemoveSubscriptions(params ICryptoCompareSubscription[] subscriptions)
        {
            var message = new RemoveSubscriptionMessage(subscriptions);
            await WebSocket.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)),
                WebSocketMessageType.Text, true, _cancellationTokenSource.Token);
        }

    }
}
