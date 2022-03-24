using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model
{
#nullable disable
    public class TopOfOrderBook : InboundMessageBase
    {
        [JsonPropertyName("M")]
        public string M { get; set; }

        [JsonPropertyName("FSYM")]
        public string FSYM { get; set; }

        [JsonPropertyName("TSYM")]
        public string TSYM { get; set; }

        [JsonPropertyName("CCSEQ")]
        public int CCSEQ { get; set; }

        [JsonPropertyName("BID")]
        public List<Bid> Bids { get; set; }
    }

    public class Bid
    {
        [JsonPropertyName("P")]
        public double P { get; set; }

        [JsonPropertyName("Q")]
        public double Q { get; set; }

        [JsonPropertyName("REPORTEDNS")]
        public string ReportedNs { get; set; }
    }
#nullable restore
}