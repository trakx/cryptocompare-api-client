using System.Collections.Generic;
using JetBrains.Annotations;
using Trakx.CryptoCompare.ApiClient.Rest.Helpers;

namespace Trakx.CryptoCompare.ApiClient.Rest.Extensions
{
    internal static class EnumerableExtensions
    {
        public static string ToJoinedList([NotNull] this IEnumerable<string> list)
        {
            Check.NotEmpty(list, nameof(list));
            return string.Join(",", list);
        }
    }
}
