using System.Collections.Generic;
using System.Collections.Immutable;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class MiningEquipmentsResponse : BaseApiResponse
    {
        public IReadOnlyDictionary<string, MiningEquipment> MiningData { get; set; } = ImmutableDictionary<string, MiningEquipment>.Empty;
    }
}
