using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model;

public class LoadComplete : InboundMessageBase
{
#nullable disable
    [JsonPropertyName("MESSAGE")] public string Message { get; set; }
    [JsonPropertyName("SUB")] public string Subscription { get; set; }
#nullable restore
}
