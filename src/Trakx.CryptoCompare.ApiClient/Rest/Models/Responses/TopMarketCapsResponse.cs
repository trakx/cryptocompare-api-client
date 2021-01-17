using System.Collections.Generic;
#pragma warning disable 8618

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class TopMarketCapResponse : BaseApiResponse
    {
        public IReadOnlyList<TopMarketCapInfo> Data { get; set; }
    }
}
