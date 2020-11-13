using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Trakx.CryptoCompare.ApiClient.Rest.Core;
using Trakx.CryptoCompare.ApiClient.Rest.Models.Responses;

namespace Trakx.CryptoCompare.ApiClient.Rest.Clients
{
    public class NewsClient : BaseApiClient, INewsClient
    {
        public NewsClient([NotNull] HttpClient httpClient)
            : base(httpClient)
        {
        }

        public async Task<IEnumerable<NewsEntity>> News(
            string lang = null,
            long? lTs = null,
            string[] feeds = null,
            bool? sign = null)
        {
            return await this.GetAsync<IEnumerable<NewsEntity>>(ApiUrls.News(lang, lTs, feeds, sign)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<NewsProvider>> NewsProviders()
        {
            return await this.GetAsync<IEnumerable<NewsProvider>>(ApiUrls.NewsProviders()).ConfigureAwait(false);
        }
    }
}
