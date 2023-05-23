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

    protected override Uri ClientSocketUri => _config.WebSocketEndpoint;

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
        if (HasRemoveAction(subscription))
        {
            return await RemoveAsync(subscription);
        }
        return await base.AddInternalAsync(subscription);
    }

    public async Task<bool> RemoveAsync(TopicSubscription subscription)
    {
        var payload = JsonSerializer.Deserialize<CryptoCompareSubscription>(subscription.Topic);

        if (payload == null) return false;
        if (payload.Action != SubscribeActions.SubRemove) return false;

        foreach (var item in payload.Subs)
        {
            var toRemoveSubs = Subscriptions.Where(t => t.Topic.ContainsIgnoreCase(item)).ToList();
            foreach (var unwanted in toRemoveSubs)
            {
                Subscriptions.Remove(unwanted);
            }
        }

        SendClient(subscription.Topic);

        await Task.CompletedTask;
        return true;
    }

    private static bool HasRemoveAction(TopicSubscription subscription)
    {
        var remove = SubscribeActions.SubRemove.ToString();
        return subscription.Topic.ContainsIgnoreCase(remove);
    }
}
