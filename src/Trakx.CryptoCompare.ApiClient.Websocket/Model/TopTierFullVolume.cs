using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model;

public class TopTierFullVolume : InboundMessageBase
{
    [JsonPropertyName("SYMBOL")]
    public string Symbol { get; set; } = default!;

    [JsonPropertyName("TOPTIERFULLVOLUME")]
    public decimal Volume { get; set; }
}
