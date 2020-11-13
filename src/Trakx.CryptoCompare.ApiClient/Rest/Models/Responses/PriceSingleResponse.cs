using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class PriceSingleResponse : ReadOnlyDictionary<string, decimal>
    {
        public PriceSingleResponse(IDictionary<string, decimal> dictionary)
            : base(dictionary)
        {
        }
    }
}
