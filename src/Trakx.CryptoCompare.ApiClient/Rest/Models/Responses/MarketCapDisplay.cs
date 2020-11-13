using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class MarketCapDisplay : ReadOnlyDictionary<string, CoinFullAggregatedDataDisplay>
    {
        public MarketCapDisplay(IDictionary<string, CoinFullAggregatedDataDisplay> dictionary)
            : base(dictionary)
        {
        }
    }
}