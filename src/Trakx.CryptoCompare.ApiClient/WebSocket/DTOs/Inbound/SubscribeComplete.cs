using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound
{
    public class SubscribeComplete : InboundMessageBase
    {
#nullable disable
        internal const string TypeValue = "16";
        [JsonPropertyName("MESSAGE")] public string Message { get; set; }
        [JsonPropertyName("SUB")] public string Subscription { get; set; }
#nullable restore
    }
}