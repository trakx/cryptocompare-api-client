using Newtonsoft.Json;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class TopVolume24HInfo
    {
        public CoinInfo CoinInfo { get; set; }

        [JsonProperty("DISPLAY")]
        public Volume24HDisplay Display { get; set; }

        [JsonProperty("RAW")]
        public Volume24HRaw Raw { get; set; }
    }
}
