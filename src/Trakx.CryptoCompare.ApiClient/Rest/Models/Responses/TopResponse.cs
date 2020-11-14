using System.Collections.Generic;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class TopResponse : BaseApiResponse
    {
#nullable disable
        public IReadOnlyList<TopInfo> Data { get; set; }
#nullable restore
    }
}
