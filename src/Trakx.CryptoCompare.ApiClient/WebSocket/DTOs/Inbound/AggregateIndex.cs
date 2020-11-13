using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound
{
    public class AggregateIndex : Ticker
    {
        internal const string TypeValue = "5";
        [JsonPropertyName("MEDIAN")] public decimal? Median { get; set; }
        [JsonPropertyName("LASTMARKET")] public string? LastMarket { get; set; }
        [JsonPropertyName("TOPTIERVOLUME24HOUR")] public decimal? TopTierVolume24Hour { get; set; }
        [JsonPropertyName("TOPTIERVOLUME24HOURTO")] public decimal? TopTierVolume24HourTo { get; set; }
    }
}