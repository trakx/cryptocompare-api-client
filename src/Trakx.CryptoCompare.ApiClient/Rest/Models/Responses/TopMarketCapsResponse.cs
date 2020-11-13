using System.Collections.Generic;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class TopMarketCapResponse : BaseApiResponse
    {
        public IReadOnlyList<TopMarketCapInfo> Data { get; set; }
    }
}
