using System.Text.Json.Serialization;
using Trakx.WebSockets;

namespace Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound
{
    public class InboundMessageBase :IBaseInboundMessage
    {
        [JsonPropertyName("TYPE")] public string Type { get; set; }
    }
}