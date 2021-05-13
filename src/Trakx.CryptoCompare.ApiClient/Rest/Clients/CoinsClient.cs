using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Trakx.CryptoCompare.ApiClient.Rest.Core;
using Trakx.CryptoCompare.ApiClient.Rest.Models.Responses;

namespace Trakx.CryptoCompare.ApiClient.Rest.Clients
{
    /// <summary>
    /// The coins client. Gets general info for all the coins available on the website.
    /// </summary>
    /// <seealso cref="T:CryptoCompare.Clients.ICoinsClient"/>
    public class CoinsClient : BaseApiClient, ICoinsClient
    {
        /// <summary>
        /// Initializes a new instance of the CryptoCompare.Clients.CoinsClient class.
        /// </summary>
        /// <param name="httpClient">The HTTP client. This cannot be null.</param>
        public CoinsClient([NotNull] HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <summary>
        /// Returns all the coins that CryptoCompare has added to the website.
        /// </summary>
        /// <seealso cref="M:CryptoCompare.Clients.ICoinsClient.AllCoinsAsync()"/>
        public async Task<CoinListResponse> ListAsync()
        {
            return await this.GetAsync<CoinListResponse>(ApiUrls.AllCoins()).ConfigureAwait(false);
        }
    }
}
