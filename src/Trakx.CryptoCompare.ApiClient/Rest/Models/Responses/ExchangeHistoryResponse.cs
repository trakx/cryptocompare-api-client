using System.Collections.Generic;
#pragma warning disable 8618

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class ExchangeHistoryResponse : BaseApiResponse
    {
        public IReadOnlyList<ExchangeHistoryData> Data { get; set; }
    }
}