using Newtonsoft.Json;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class TopMarketCapInfo
    {
        public CoinInfo CoinInfo { get; set; }

        [JsonProperty("DISPLAY")]
        public MarketCapDisplay Display { get; set; }

        [JsonProperty("RAW")]
        public MarketCapRaw Raw { get; set; }
    }
}
