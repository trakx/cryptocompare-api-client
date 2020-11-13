using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound
{
    public class UnsubscribeAllComplete : InboundMessageBase
    {
        internal const string TypeValue = "18";
        [JsonPropertyName("MESSAGE")] public string Message { get; set; }
        [JsonPropertyName("PARAMETER")] public string Parameter { get; set; }
        [JsonPropertyName("INFO")] public string Info { get; set; }
    }
}