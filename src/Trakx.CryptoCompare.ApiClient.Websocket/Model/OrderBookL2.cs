using System.Text.Json.Serialization;

#pragma warning disable 8618
namespace Trakx.CryptoCompare.ApiClient.Websocket.Model;

public class OrderBookL2 : InboundMessageBase
{
    [JsonPropertyName("M")]
    public string M { get; set; }

    [JsonPropertyName("FSYM")]
    public string FSYM { get; set; }

    [JsonPropertyName("TSYM")]
    public string TSYM { get; set; }

    [JsonPropertyName("SIDE")]
    public int Side { get; set; }

    [JsonPropertyName("ACTION")]
    public int Action { get; set; }

    [JsonPropertyName("CCSEQ")]
    public long CCSEQ { get; set; }

    [JsonPropertyName("P")]
    public double P { get; set; }

    [JsonPropertyName("Q")]
    public int Q { get; set; }

    [JsonPropertyName("SEQ")]
    public long Seq { get; set; }

    [JsonPropertyName("REPORTEDNS")]
    public long ReportedDns { get; set; }

    [JsonPropertyName("DELAYNS")]
    public int DelayNS { get; set; }
}
