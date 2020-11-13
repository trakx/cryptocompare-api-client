using System;
using Newtonsoft.Json;
using Trakx.CryptoCompare.ApiClient.Rest.Converters;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class ExchangeHistoryData
    {
        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTimeOffset Time { get; set; }

        public decimal Volume { get; set; }
    }
}