using System.Collections.Generic;
using System.Collections.Immutable;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class TopVolumesResponse : BaseApiResponse
    {
        public IReadOnlyList<TopVolumeInfo> Data { get; set; } = ImmutableList<TopVolumeInfo>.Empty;

        public string? VolSymbol { get; set; }
    }
}
