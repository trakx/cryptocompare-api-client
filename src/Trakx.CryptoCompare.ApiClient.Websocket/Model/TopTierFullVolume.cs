using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model;

public class TopTierFullVolume : InboundMessageBase
{
#nullable disable
    [JsonPropertyName("SYMBOL")] public string Symbol { get; set; }
    [JsonPropertyName("TOPTIERFULLVOLUME")] public string Volume { get; set; }
#nullable restore
}
