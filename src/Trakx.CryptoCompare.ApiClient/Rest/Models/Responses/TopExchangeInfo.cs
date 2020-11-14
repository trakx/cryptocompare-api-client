using System.Collections.Generic;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class TopExchangeInfo
    {
#nullable disable
        public CoinInfo CoinInfo { get; set; }

        public AggregatedData AggregatedData { get; set; }

        public IEnumerable<CoinFullAggregatedDataDisplay> Exchanges { get; set; }
#nullable restore
    }
}