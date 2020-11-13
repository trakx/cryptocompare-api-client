using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound
{
    public class InboundMessageBase
    {
        [JsonPropertyName("TYPE")] public string Type { get; set; }
    }
}