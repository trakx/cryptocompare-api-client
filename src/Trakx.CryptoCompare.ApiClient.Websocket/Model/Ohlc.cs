using System.Text.Json.Serialization;
using Trakx.Utils.Serialization.Converters;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model
{
    public class Ohlc : InboundMessageBase
    {
#nullable disable
        [JsonPropertyName("MARKET")] public string Market { get; set; }
        [JsonPropertyName("FROMSYMBOL")] public string FromSymbol { get; set; }
        [JsonPropertyName("TOSYMBOL")] public string ToSymbol { get; set; }
        [JsonPropertyName("TS"), JsonConverter(typeof(ULongOrStringConverter))] public ulong TimeStamp { get; set; }
        [JsonPropertyName("UNIT")] public string Unit { get; set; }
        [JsonPropertyName("ACTION")] public string Action { get; set; }
        [JsonPropertyName("OPEN")] public decimal Open { get; set; }
        [JsonPropertyName("HIGH")] public decimal High { get; set; }
        [JsonPropertyName("LOW")] public decimal Low { get; set; }
        [JsonPropertyName("CLOSE")] public decimal Close { get; set; }
        [JsonPropertyName("VOLUMEFROM")] public decimal VolumeFrom { get; set; }
        [JsonPropertyName("VOLUMETO")] public decimal VolumeTo { get; set; }
        [JsonPropertyName("TOTALTRADES"), JsonConverter(typeof(ULongOrStringConverter))] public ulong TotalTrades { get; set; }
        [JsonPropertyName("FIRSTTS"), JsonConverter(typeof(ULongOrStringConverter))] public ulong FirstTimeStamp { get; set; }
        [JsonPropertyName("LASTTS"), JsonConverter(typeof(ULongOrStringConverter))] public ulong LastTimeStamp { get; set; }
        [JsonPropertyName("FIRSTPRICE")] public decimal FirstPrice { get; set; }
        [JsonPropertyName("MAXPRICE")] public decimal MaxPrice { get; set; }
        [JsonPropertyName("MINPRICE")] public decimal MinPrice { get; set; }
        [JsonPropertyName("LASTPRICE")] public decimal LastPrice { get; set; }
#nullable restore
    }
}