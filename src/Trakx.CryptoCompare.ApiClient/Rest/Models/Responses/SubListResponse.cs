﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Trakx.CryptoCompare.ApiClient.Rest.Converters;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class SubListResponse : ReadOnlyDictionary<string, SubList>
    {
        public SubListResponse(IDictionary<string, SubList> dictionary)
            : base(dictionary)
        {
        }
    }

    public class SubList
    {
        [JsonConverter(typeof(StringToSubConverter))]
        public IReadOnlyList<Sub>? Current { get; set; }

        [JsonConverter(typeof(StringToSubConverter))]
        public Sub? CurrentAgg { get; set; }

        [JsonConverter(typeof(StringToSubConverter))]
        public IReadOnlyList<Sub>? Trades { get; set; }
    }
}
