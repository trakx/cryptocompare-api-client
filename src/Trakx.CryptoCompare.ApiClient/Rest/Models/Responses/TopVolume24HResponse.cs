using System.Collections.Generic;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class TopVolume24HResponse : BaseApiResponse
    {
        public IReadOnlyList<TopVolume24HInfo> Data { get; set; }
    }
}
