using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class PriceMultiResponse : ReadOnlyDictionary<string, IReadOnlyDictionary<string, decimal>>
    {
        public PriceMultiResponse(IDictionary<string, IReadOnlyDictionary<string, decimal>> dictionary)
            : base(dictionary)
        {
        }
    }
}
