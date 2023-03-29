namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class TopInfo
    {
#nullable disable
        public string Exchange { get; set; }

        public string BaseSymbol { get; set; }

        public string QuoteSymbol { get; set; }

        public decimal Volume24H { get; set; }

        public decimal Volume24HTo { get; set; }
#nullable restore
    }
}
