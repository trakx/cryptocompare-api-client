﻿using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
using Trakx.CryptoCompare.ApiClient.Rest.Extensions;
using Trakx.CryptoCompare.ApiClient.Rest.Helpers;
using Trakx.CryptoCompare.ApiClient.Rest.Models.Responses;

namespace Trakx.CryptoCompare.ApiClient.Rest.Core
{
    internal static class ApiUrls
    {
        private const string RateLimitsUrl = "/stats/rate/{0}/limit";

        public static readonly Uri MinApiEndpoint = new(
            "https://min-api.cryptocompare.com/data/",
            UriKind.Absolute);

        public static readonly Uri SiteApiEndpoint = new(
            "https://www.cryptocompare.com/api/data/",
            UriKind.Absolute);

        public static Uri AllCoins() => new(MinApiEndpoint, "all/coinlist");

        public static Uri AllExchanges() => new(MinApiEndpoint, "all/exchanges");

        public static Uri DayAveragePrice(
            string fsym,
            string tsym,
            string? e,
            DateTimeOffset? toTs,
            CalculationType? avgType,
            int? UTCHourDiff,
            bool? tryConversion)
        {
            return new Uri(MinApiEndpoint, "dayAvg").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsym), tsym },
                    { nameof(e), e },
                    { nameof(toTs), toTs?.ToString() },
                    { nameof(avgType), avgType?.ToString() },
                    { nameof(UTCHourDiff), UTCHourDiff?.ToString() },
                    { nameof(tryConversion), tryConversion?.ToString().ToLowerInvariant() },
                });
        }

        public static Uri History(
            string method,
            string fsym,
            string tsym,
            int? limit,
            string? e,
            DateTimeOffset? toTs,
            bool? allData,
            int? aggregate,
            bool? tryConversion)
        {
            Check.NotNullOrWhiteSpace(fsym, nameof(fsym));
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));

            return new Uri(MinApiEndpoint, $"v2/histo{method}").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsym), tsym },
                    { nameof(limit), limit.ToString() },
                    { nameof(toTs), toTs?.ToUnixTime().ToString(CultureInfo.InvariantCulture) },
                    { nameof(tryConversion), tryConversion?.ToString().ToLowerInvariant() },
                    { nameof(e), e },
                    { nameof(allData), allData?.ToString().ToLowerInvariant() },
                    { nameof(aggregate), aggregate?.ToString() }
                });
        }

        public static Uri ExchangeHistory(
            string method,
            string tsym,
            string? e,
            DateTimeOffset? toTs,
            int? limit,
            int? aggregate,
            bool? aggregatePredictableTimePeriods)
        {
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));

            return new Uri(MinApiEndpoint, $"exchange/histo{method}").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(tsym), tsym },
                    { nameof(limit), limit.ToString() },
                    { nameof(toTs), toTs?.ToUnixTime().ToString(CultureInfo.InvariantCulture) },
                    { nameof(e), e },
                    { nameof(aggregate), aggregate?.ToString() },
                    { nameof(aggregatePredictableTimePeriods), aggregatePredictableTimePeriods?.ToString().ToLowerInvariant() }
                });
        }

        public static Uri MiningContracts() => new(SiteApiEndpoint, "miningcontracts");

        public static Uri MiningEquipments() => new(SiteApiEndpoint, "miningequipment");

        public static Uri News(string? lang = null, long? lTs = null, string[]? feeds = null, bool? sign = null)
        {
            return new Uri(MinApiEndpoint, "news/").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(lang), lang },
                    { nameof(lTs), lTs?.ToString() },
                    { nameof(feeds), feeds != null ? string.Join(",", feeds) : null },
                    { nameof(sign), sign?.ToString().ToLowerInvariant() },
                });
        }

        public static Uri NewsProviders()
        {
            return new Uri(MinApiEndpoint, "news/providers");
        }

        public static Uri PriceAverage(
            [NotNull] string fsym,
            [NotNull] string tsym,
            [NotNull] IEnumerable<string> e,
            bool? tryConversion)
        {
            Check.NotNullOrWhiteSpace(fsym, nameof(fsym));
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));
            Check.NotEmpty(e, nameof(e));

            return new Uri(MinApiEndpoint, "generateAvg").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsym), tsym },
                    { nameof(e), e?.ToJoinedList() },
                    { nameof(tryConversion), tryConversion?.ToString().ToLowerInvariant() }
                });
        }

        public static Uri PriceHistorical(
            string fsym,
            IEnumerable<string> tsyms,
            IEnumerable<string>? markets,
            DateTimeOffset ts,
            CalculationType? calculationType,
            bool? tryConversion)
        {
            return new Uri(MinApiEndpoint, "pricehistorical").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsyms), tsyms.ToJoinedList() },
                    { nameof(ts), ts.ToUnixTime().ToString(CultureInfo.InvariantCulture) },
                    { nameof(markets), markets?.ToJoinedList() },
                    { nameof(calculationType), calculationType?.ToString("G") },
                    { nameof(tryConversion), tryConversion?.ToString().ToLowerInvariant() }
                });
        }

        public static Uri PriceMulti(
            IEnumerable<string> fsyms,
            IEnumerable<string> tsyms,
            bool? tryConversion,
            string? e)
        {
            Check.NotEmpty(fsyms, nameof(fsyms));
            Check.NotEmpty(tsyms, nameof(tsyms));

            return new Uri(MinApiEndpoint, "pricemulti").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(fsyms), fsyms.ToJoinedList() },
                    { nameof(tsyms), tsyms.ToJoinedList() },
                    { nameof(tryConversion), tryConversion?.ToString().ToLowerInvariant() },
                    { nameof(e), e }
                });
        }

        public static Uri PriceMultiFull(
            IEnumerable<string> fsyms,
            IEnumerable<string> tsyms,
            bool? tryConversion,
            string? e)
        {
            Check.NotEmpty(fsyms, nameof(fsyms));
            Check.NotEmpty(tsyms, nameof(tsyms));

            return new Uri(MinApiEndpoint, "pricemultifull").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(fsyms), fsyms.ToJoinedList() },
                    { nameof(tsyms), tsyms.ToJoinedList() },
                    { nameof(tryConversion), tryConversion?.ToString().ToLowerInvariant() },
                    { nameof(e), e }
                });
        }

        public static Uri PriceSingle(
            string fsym,
            IEnumerable<string> tsyms,
            bool? tryConversion,
            string? e)
        {
            Check.NotNullOrWhiteSpace(fsym, nameof(fsym));
            Check.NotEmpty(tsyms, nameof(tsyms));

            return new Uri(MinApiEndpoint, "price").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsyms), tsyms.ToJoinedList() },
                    { nameof(tryConversion), tryConversion?.ToString().ToLowerInvariant() },
                    { nameof(e), e }
                });
        }

        public static Uri RateLimitsByHour() => new(MinApiEndpoint, string.Format(RateLimitsUrl, "hour"));

        public static Uri RateLimitsByMinute() => new(MinApiEndpoint, string.Format(RateLimitsUrl, "minute"));

        public static Uri RateLimitsBySecond() => new(MinApiEndpoint, string.Format(RateLimitsUrl, "second"));

        public static Uri SocialStats([NotNull] int id)
        {
            Check.NotNull(id, nameof(id));
            return new Uri(SiteApiEndpoint, "socialstats").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(id), id.ToString() }
                });
        }

        public static Uri SubsList([NotNull] string fsym, [NotNull] IEnumerable<string> tsyms)
        {
            Check.NotEmpty(tsyms, nameof(tsyms));
            Check.NotNull(fsym, nameof(fsym));
            return new Uri(MinApiEndpoint, "subs").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsyms), tsyms.ToJoinedList() }
                });
        }

        public static Uri TopExchangesVolumeDataByPair([NotNull] string fsym, [NotNull] string tsym, int? limit)
        {
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));
            Check.NotNullOrWhiteSpace(fsym, nameof(fsym));
            return new Uri(MinApiEndpoint, "top/exchanges").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsym), tsym },
                    { nameof(limit), limit.ToString() }
                });
        }

        public static Uri TopOfTradingPairs([NotNull] string fsym, int? limit)
        {
            Check.NotNullOrWhiteSpace(fsym, nameof(fsym));
            return new Uri(MinApiEndpoint, "top/pairs").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(fsym), fsym },
                    { nameof(limit), limit.ToString() }
                });
        }

        public static Uri TopByPairVolume([NotNull] string tsym, int? limit)
        {
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));
            return new Uri(MinApiEndpoint, "top/volumes").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(tsym), tsym },
                    { nameof(limit), limit.ToString() }
                });
        }

        public static Uri TopByVolume24HFull([NotNull] string tsym, int? limit, int? page, bool? sign)
        {
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));
            return new Uri(MinApiEndpoint, "top/totalvolfull").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(tsym), tsym },
                    { nameof(limit), limit?.ToString() },
                    { nameof(page), page?.ToString() },
                    { nameof(sign), sign?.ToString().ToLowerInvariant() }
                });
        }

        public static Uri TopByMarketCapFull([NotNull] string tsym, int? limit, int? page, bool? sign)
        {
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));
            return new Uri(MinApiEndpoint, "top/mktcapfull").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(tsym), tsym },
                    { nameof(limit), limit?.ToString() },
                    { nameof(page), page?.ToString() },
                    { nameof(sign), sign?.ToString().ToLowerInvariant() }
                });
        }

        public static Uri ExchangesFullDataByPair(string fsym, string tsym, int? limit)
        {
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));
            Check.NotNullOrWhiteSpace(fsym, nameof(fsym));
            return new Uri(MinApiEndpoint, "top/exchanges/full").ApplyParameters(
                new Dictionary<string, string?>
                {
                    { nameof(tsym), tsym },
                    { nameof(fsym), fsym },
                    { nameof(limit), limit?.ToString() }
                });
        }
    }
}
