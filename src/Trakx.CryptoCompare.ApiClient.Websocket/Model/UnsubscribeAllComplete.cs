using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model;

public class UnsubscribeAllComplete : InboundMessageBase
{
#nullable disable
    [JsonPropertyName("MESSAGE")] public string Message { get; set; }
    [JsonPropertyName("PARAMETER")] public string Parameter { get; set; }
    [JsonPropertyName("INFO")] public string Info { get; set; }
#nullable restore
}
