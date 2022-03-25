using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Trakx.CryptoCompare.ApiClient.Websocket.Model;
using Trakx.Websocket;
using Trakx.Websocket.Interfaces;
using Trakx.Websocket.Model;

namespace Trakx.CryptoCompare.ApiClient.Websocket;

public class CryptoCompareWebsocketHandler : ClientWebsocketRedirectHandlerBase<InboundMessageBase>, ICryptoCompareWebsocketHandler
{
    private readonly CryptoCompareApiConfiguration _config;

    public CryptoCompareWebsocketHandler(WebsocketConfiguration websocketConfigurationOption,
        CryptoCompareApiConfiguration config,
        IClientWebsocketFactory clientWebsocketFactory,
        TaskScheduler taskScheduler = null) : base(
        websocketConfigurationOption
        , clientWebsocketFactory, taskScheduler)
    {
        _config = config;
    }

    protected override IReadOnlyDictionary<string, Type> MessageTypesByTopics => new Dictionary<string, Type>
    {
        { "0", typeof(Trade) },
        { "20", typeof(Welcome) },
        { "17", typeof(UnsubscribeComplete) },
        { "18", typeof(UnsubscribeAllComplete) },
        { "2", typeof(Ticker) },
        { "16", typeof(SubscribeComplete) },
        { "24", typeof(Ohlc) },
        { "3", typeof(LoadComplete) },
        { "999", typeof(HeartBeat) },
        { "500", typeof(Error) },
        { "5", typeof(AggregateIndex) },
        { "11", typeof(FullVolume) },
        { "30", typeof(TopOfOrderBook) },
        { "21", typeof(TopTierFullVolume) },
        { "8", typeof(OrderBookL2) },
        { "401", typeof(Unauthorized) }
    };

    protected override object? DeserializeClientSocketIncoming(string topic, IncomingMessage m)
    {
        var typeStr = m.DeserializedMessage?.Type ?? null;
        if (typeStr != null && MessageTypesByTopics.ContainsKey(typeStr))
        {
            var type = MessageTypesByTopics[typeStr];
            return JsonSerializer.Deserialize(m.ResponseMessage.Text, type);
        }

        Log.Logger.Warning("Message {type} not recognised", m.ResponseMessage.Text);
        return null;
    }

    protected override async Task<bool> AddInternalAsync(TopicSubscription subscription)
    {
        var jDoc = JsonDocument.Parse(subscription.Topic);
        JsonProperty? unsubProperties = jDoc.RootElement.EnumerateObject().Any(t => t.Name.Equals("action", StringComparison.InvariantCultureIgnoreCase)
            && t.Value.GetString().Equals("SubRemove", StringComparison.InvariantCultureIgnoreCase)) ?
            jDoc.RootElement.EnumerateObject().FirstOrDefault(t => t.Name == "subs")
            : null;
        if (unsubProperties != null)
        {
            var unsubArrayLength = unsubProperties.Value.Value.GetArrayLength();
            for (int i = 0; i < unsubArrayLength; i++)
            {
                var unsubElement = unsubProperties.Value.Value[i].GetString();
                var toRemoveSubs = Subscriptions.Where(t => t.Topic.Contains(unsubElement, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
                if (toRemoveSubs.Any())
                {
                    await this.RemoveAsync(toRemoveSubs);
                }
            }
            SendClient(subscription.Topic);
            return true;
        }

        return await base.AddInternalAsync(subscription);
    }

    protected override Uri ClientSocketUri => _config.WebSocketEndpoint;
}
