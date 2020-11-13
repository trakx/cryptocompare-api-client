using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Outbound
{
    public abstract class WebSocketSubscriptionMessage
    {
        protected WebSocketSubscriptionMessage(string action, params ICryptoCompareSubscription[] subscriptions)
        {
            Action = action;
            Format = "streamer";
            var subscriptionList = new List<ICryptoCompareSubscription>(subscriptions);
            Subscriptions = subscriptionList.AsReadOnly();
        }
        
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("subs"), JsonConverter(typeof(CryptoCompareSubscriptionListConverter))]
        public IReadOnlyList<ICryptoCompareSubscription> Subscriptions { get; }

        [JsonPropertyName("format")] 
        public string Format { get; set; }
    }

    public sealed class AddSubscriptionMessage : WebSocketSubscriptionMessage
    {
        internal const string SubAdd = "SubAdd";

        /// <inheritdoc />
        public AddSubscriptionMessage(params ICryptoCompareSubscription[] subscriptions) : base(SubAdd, subscriptions) { }
    }

    public sealed class RemoveSubscriptionMessage : WebSocketSubscriptionMessage
    {
        internal const string SubRemove = "SubRemove";

        /// <inheritdoc />
        public RemoveSubscriptionMessage(params ICryptoCompareSubscription[] subscriptions) : base(SubRemove, subscriptions) { }
    }
}