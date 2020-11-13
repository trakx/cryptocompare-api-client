using System.Collections.Generic;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class TopVolumesResponse : BaseApiResponse
    {
        public IReadOnlyList<TopVolumeInfo> Data { get; set; }

        public string VolSymbol { get; set; }
    }
}
