using Newtonsoft.Json;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class PriceAverageResponse
    {
        [JsonProperty("DISPLAY")]
        public CoinFullAggregatedDataDisplay Display { get; set; }

        [JsonProperty("RAW")]
        public CoinFullAggregatedData Raw { get; set; }
    }
}
