using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using Trakx.Common.Extensions;
using Trakx.CryptoCompare.ApiClient.Websocket.Model;
using Trakx.Websocket;
using Trakx.Websocket.Interfaces;
using Trakx.Websocket.Model;

namespace Trakx.CryptoCompare.ApiClient.Websocket;

public class CryptoCompareWebsocketHandler : ClientWebsocketRedirectHandlerBase<InboundMessageBase>, ICryptoCompareWebsocketHandler
{
    private readonly CryptoCompareApiConfiguration _config;

    public CryptoCompareWebsocketHandler(
        WebsocketConfiguration websocketConfigurationOption,
        CryptoCompareApiConfiguration config,
        IClientWebsocketFactory clientWebsocketFactory,
        TaskScheduler? taskScheduler = null)
        : base(websocketConfigurationOption, clientWebsocketFactory, taskScheduler)
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
        var typeStr = m.DeserializedMessage?.Type;
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
        if (subscription.Topic.ContainsIgnoreCase(SubscribeActions.SubRemove.ToString()))
        {
            return await RemovePrivateAsync(subscription);
        }
        return await base.AddInternalAsync(subscription);
    }

    private async Task<bool> RemovePrivateAsync(TopicSubscription subscription)
    {
        var payload = JsonSerializer.Deserialize<CryptoCompareSubscription>(subscription.Topic);

        if (payload == null) return false;
        if (payload.Action != SubscribeActions.SubRemove) return false;

        foreach (var item in payload.Subs)
        {
            var toRemoveSubs = Subscriptions.Where(t => t.Topic.ContainsIgnoreCase(item));
            if (!toRemoveSubs.Any()) continue;
            await this.RemoveAsync(toRemoveSubs.ToList());
        }

        SendClient(subscription.Topic);
        return true;
    }

    protected override Uri ClientSocketUri => _config.WebSocketEndpoint;
}
