using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Outbound;
using Trakx.Websocket;
using Trakx.Websocket.Interfaces;
using Trakx.Websocket.Model;

namespace Trakx.CryptoCompare.ApiClient.WebSocket
{
    public class CryptoCompareRedirectRegistry : ClientWebsocketRedirectRegistryBase<CryptoCompareWebsocketSubscription, InboundMessageBase>
    {
        private readonly IOptions<CryptoCompareApiConfiguration> _configuration;
        private const string SubKey = "subs";

        /// <inheritdoc />
        public CryptoCompareRedirectRegistry(ILogger<BaseWebsocketRegistry<CryptoCompareWebsocketSubscription>> logger,
            IOptions<WebsocketConfiguration> websocketConfigurationOption,
            IClientWebsocketFactory clientWebsocketFactory,
            IOptions<CryptoCompareApiConfiguration> configuration,
            IList<CryptoCompareWebsocketSubscription>? subscriptions = null) :
            base(logger, websocketConfigurationOption, clientWebsocketFactory, subscriptions)
        {
            _configuration = configuration;
            AddObservable<Trade>();
            AddObservable<Ticker>();
            AddObservable<AggregateIndex>();
            AddObservable<Ohlc>();
            AddObservable<SubscribeComplete>();
            AddObservable<UnsubscribeComplete>();
            AddObservable<LoadComplete>();
            AddObservable<UnsubscribeAllComplete>();
            AddObservable<HeartBeat>();
            AddObservable<Error>();
            AddObservable<Welcome>();

            Observables.Keys
                .Select(t => (Type: t, Instance: Activator.CreateInstance(t)))
                .Select(newObject => new
                {
                    newObject!.Type,
                    TypeValue = (string)newObject!.Type
                        .GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                        .FirstOrDefault(t => t.Name == "TypeValue")?.GetValue(newObject.Instance) ?? null
                })
                .ToList()
                .ForEach(t => AddType(t.TypeValue!, t.Type));
        }

        #region Overrides of ClientWebsocketRedirectRegistryBase<CryptoCompareWebsocketSubscription,InboundMessageBase>

        /// <inheritdoc />
        public override Uri ClientSocketUri => _configuration.Value.WebSocketEndpoint;

        /// <inheritdoc />
        protected override string GetTypeName(InboundMessageBase deserializedMessage)
        {
            return deserializedMessage.Type;
        }

        protected override async Task<bool> AddInternalAsync(CryptoCompareWebsocketSubscription subscription)
        {
            if (IsUnsubscribeMessage(subscription))
            {
                await SendAsync(subscription.Topic);
                return false;
            }

            return await base.AddInternalAsync(subscription);
        }

        private bool IsUnsubscribeMessage(CryptoCompareWebsocketSubscription subscription)
        {
            var messageToSend = JObject.Parse(subscription.Topic);
            if (messageToSend.TryGetValue("action", out var action) && action.Value<string>() == "SubRemove")
            {
                var messageToSendSubsList = messageToSend[SubKey]!.ToList();
                var existingSubscriptionList = Subscriptions.Select(t => (Sub: t, Obj: JObject.Parse(t.Topic)))
                    .Where(t => t.Obj.ContainsKey(SubKey) && t.Obj[SubKey]!.ToList()
                        .Any(sub => messageToSendSubsList.Contains(sub)))
                    .ToList();
                if (existingSubscriptionList.Any())
                {
                    foreach (var existingSub in existingSubscriptionList)
                    {
                        var newSubs = existingSub.Obj[SubKey]!.ToList().Except(messageToSendSubsList)
                            .ToList();
                        if (!newSubs.Any())
                        {
                            Subscriptions.Remove(existingSub.Sub);
                        }
                        else
                        {
                            existingSub.Obj[SubKey] = JArray.FromObject(newSubs.ToList());
                            existingSub.Sub.Topic = existingSub.Obj.ToString();
                        }
                    }
                }
                return true;
            }

            return false;
        }

        #endregion
    }

}
