using System.Collections.Generic;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class MiningEquipmentsResponse : BaseApiResponse
    {
        public IReadOnlyDictionary<string, MiningEquipment> MiningData { get; set; }
    }
}
