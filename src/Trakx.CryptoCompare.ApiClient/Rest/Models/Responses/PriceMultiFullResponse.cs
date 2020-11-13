using Newtonsoft.Json;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class PriceMultiFullResponse
    {
        [JsonProperty("DISPLAY")]
        public PriceMultiFullDisplay Display { get; set; }

        [JsonProperty("RAW")]
        public PriceMultiFullRaw Raw { get; set; }
    }
}
