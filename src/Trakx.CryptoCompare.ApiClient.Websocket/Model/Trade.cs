using System.Text.Json.Serialization;
using Trakx.Utils.Serialization.Converters;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model;

public class Trade : InboundMessageBase
{
#nullable disable
    [JsonPropertyName("M")] public string Market { get; set; }
    [JsonPropertyName("FSYM")] public string FromSymbol { get; set; }
    [JsonPropertyName("TSYM")] public string ToSymbol { get; set; }
    [JsonPropertyName("F"), JsonConverter(typeof(ULongOrStringConverter))] public ulong Flags { get; set; }
    [JsonPropertyName("ID")] public string Id { get; set; }
    [JsonPropertyName("TS"), JsonConverter(typeof(ULongOrStringConverter))] public ulong TimeStamp { get; set; }
    [JsonPropertyName("Q")] public decimal Quantity { get; set; }
    [JsonPropertyName("P")] public decimal Price { get; set; }
    [JsonPropertyName("TOTAL")] public decimal Total { get; set; }
    [JsonPropertyName("RTS"), JsonConverter(typeof(ULongOrStringConverter))] public ulong RTimeStamp { get; set; }
#nullable restore
}
