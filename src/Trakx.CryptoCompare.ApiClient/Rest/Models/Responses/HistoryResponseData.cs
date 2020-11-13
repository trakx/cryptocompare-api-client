using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Trakx.CryptoCompare.ApiClient.Rest.Converters;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class HistoryResponseData
    {
        public bool Aggregated { get; set; }

        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTimeOffset TimeFrom { get; set; }

        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTimeOffset TimeTo { get; set; }

        public IReadOnlyList<CandleData> Data { get; set; }
    }
}