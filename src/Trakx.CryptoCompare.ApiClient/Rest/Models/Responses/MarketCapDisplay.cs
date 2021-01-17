using System.Collections.Generic;
using System.Collections.ObjectModel;
#pragma warning disable 8618

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