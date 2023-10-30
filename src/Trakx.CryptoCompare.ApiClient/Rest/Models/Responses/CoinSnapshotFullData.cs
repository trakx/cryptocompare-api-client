﻿using System.Collections.Generic;
using System.Collections.Immutable;
using Newtonsoft.Json;
using Trakx.CryptoCompare.ApiClient.Rest.Converters;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class CoinSnapshotFullData
    {
        public CoinGeneralInfo? General { get; set; }

        public ICO? ICO { get; set; }

        public SEO? SEO { get; set; }

        public IReadOnlyList<string> StreamerDataRaw { get; set; } = ImmutableList<string>.Empty;

        [JsonConverter(typeof(StringToSubConverter))]
        public IReadOnlyList<Sub> Subs { get; set; } = ImmutableList<Sub>.Empty;
    }
}
