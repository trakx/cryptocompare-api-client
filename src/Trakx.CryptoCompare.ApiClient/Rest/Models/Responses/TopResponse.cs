using System.Collections.Generic;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class TopResponse : BaseApiResponse
    {
        public IReadOnlyList<TopInfo> Data { get; set; }
    }
}
