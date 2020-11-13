using System.Collections.Generic;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class MiningContractsResponse : BaseApiResponse
    {
        public IReadOnlyDictionary<string, MiningContract> MiningData { get; set; }
    }
}
