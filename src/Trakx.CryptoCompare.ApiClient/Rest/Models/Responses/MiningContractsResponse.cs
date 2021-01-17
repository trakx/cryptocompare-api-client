using System.Collections.Generic;
#pragma warning disable 8618

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class MiningContractsResponse : BaseApiResponse
    {
        public IReadOnlyDictionary<string, MiningContract> MiningData { get; set; }
    }
}
