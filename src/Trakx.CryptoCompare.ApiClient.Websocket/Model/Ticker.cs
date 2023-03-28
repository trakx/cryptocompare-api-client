using System.Text.Json.Serialization;
using Trakx.Utils.Serialization.Converters;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model;

public class Ticker : InboundMessageBase
{
#nullable disable
    [JsonPropertyName("MARKET")] public string Market { get; set; }
    [JsonPropertyName("FROMSYMBOL")] public string BaseSymbol { get; set; }
    [JsonPropertyName("TOSYMBOL")] public string QuoteSymbol { get; set; }

    [JsonPropertyName("FLAGS"), JsonConverter(typeof(ULongOrStringConverter))]
    public ulong Flags { get; set; }

    [JsonPropertyName("PRICE")] public decimal? Price { get; set; }
    [JsonPropertyName("LASTUPDATE")] public long? LastUpdate { get; set; }
    [JsonPropertyName("LASTVOLUME")] public decimal? LastVolume { get; set; }
    [JsonPropertyName("LASTVOLUMETO")] public decimal? LastVolumeTo { get; set; }
    [JsonPropertyName("LASTTRADEID")] public string? LastTradeId { get; set; }
    [JsonPropertyName("VOLUMEDAY")] public decimal VolumeDay { get; set; }
    [JsonPropertyName("VOLUMEDAYTO")] public decimal VolumeDayTo { get; set; }
    [JsonPropertyName("VOLUME24HOUR")] public decimal Volume24Hour { get; set; }
    [JsonPropertyName("VOLUME24HOURTO")] public decimal Volume24HourTo { get; set; }
    [JsonPropertyName("OPENDAY")] public decimal? OpenDay { get; set; }
    [JsonPropertyName("HIGHDAY")] public decimal? HighDay { get; set; }
    [JsonPropertyName("LOWDAY")] public decimal? LowDay { get; set; }
    [JsonPropertyName("OPEN24HOUR")] public decimal? Open24Hour { get; set; }
    [JsonPropertyName("HIGH24HOUR")] public decimal? High24Hour { get; set; }
    [JsonPropertyName("LOW24HOUR")] public decimal? Low24Hour { get; set; }
    [JsonPropertyName("VOLUMEHOUR")] public decimal VolumeHour { get; set; }
    [JsonPropertyName("VOLUMEHOURTO")] public decimal VolumeHourTo { get; set; }
    [JsonPropertyName("OPENHOUR")] public decimal? OpenHour { get; set; }
    [JsonPropertyName("HIGHHOUR")] public decimal? HighHour { get; set; }
    [JsonPropertyName("LOWHOUR")] public decimal? LowHour { get; set; }
#nullable restore
}
