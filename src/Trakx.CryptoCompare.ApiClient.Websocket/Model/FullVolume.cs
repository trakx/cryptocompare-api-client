using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model
{
    public class FullVolume : InboundMessageBase
    {
#nullable disable
        [JsonPropertyName("SYMBOL")] public string Symbol { get; set; }
        [JsonPropertyName("FULLVOLUME")] public decimal Volume { get; set; }
#nullable restore
    }
}