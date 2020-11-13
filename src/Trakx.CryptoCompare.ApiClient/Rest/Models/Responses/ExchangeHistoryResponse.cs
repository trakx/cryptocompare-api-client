using System.Collections.Generic;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class ExchangeHistoryResponse : BaseApiResponse
    {
        public IReadOnlyList<ExchangeHistoryData> Data { get; set; }
    }
}