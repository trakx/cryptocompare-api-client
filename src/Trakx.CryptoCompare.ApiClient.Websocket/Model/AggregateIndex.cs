using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model
{
    public class AggregateIndex : Ticker
    {
        [JsonPropertyName("MEDIAN")] public decimal? Median { get; set; }
        [JsonPropertyName("LASTMARKET")] public string? LastMarket { get; set; }
        [JsonPropertyName("TOPTIERVOLUME24HOUR")] public decimal? TopTierVolume24Hour { get; set; }
        [JsonPropertyName("TOPTIERVOLUME24HOURTO")] public decimal? TopTierVolume24HourTo { get; set; }
    }
}