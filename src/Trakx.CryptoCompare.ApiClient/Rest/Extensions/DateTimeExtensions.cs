﻿using System;
using Trakx.Common.Extensions;

namespace Trakx.CryptoCompare.ApiClient.Rest.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTimeOffset epoch = (1970, 1, 1).ToDateTimeOffset();

        /// <summary>
        /// Convert a Unix tick to a <see cref="DateTimeOffset"/> with UTC offset
        /// </summary>
        /// <param name="unixTime">UTC tick</param>
        public static DateTimeOffset FromUnixTime(this long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Convert <see cref="DateTimeOffset"/> with UTC offset to a Unix tick
        /// </summary>
        /// <param name="date">Date Time with UTC offset</param>
        public static long ToUnixTime(this DateTimeOffset date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }
    }
}
