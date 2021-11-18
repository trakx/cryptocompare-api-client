using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model
{
    public class CryptoCompareSubscription
    {
        [JsonPropertyName("action")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SubscribeActions Action { get; set; }

        [JsonPropertyName("subs")]
        public IList<string> Subs { get; set; }
    }
}
