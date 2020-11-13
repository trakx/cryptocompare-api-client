using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class MarketCapRaw : ReadOnlyDictionary<string, CoinFullAggregatedData>
    {
        public MarketCapRaw(IDictionary<string, CoinFullAggregatedData> dictionary)
            : base(dictionary)
        {
        }
    }
}