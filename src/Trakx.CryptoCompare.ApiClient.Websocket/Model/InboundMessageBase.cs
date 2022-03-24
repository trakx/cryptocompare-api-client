using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model;

public class InboundMessageBase
{
    [JsonPropertyName("TYPE")] public string Type { get; set; }
}
