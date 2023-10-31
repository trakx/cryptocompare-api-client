using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model;

public class HeartBeat : InboundMessageBase
{
    [JsonPropertyName("MESSAGE")] public string? Message { get; set; }
    [JsonPropertyName("TIMEMS")] public ulong? TimeMs { get; set; }
}
