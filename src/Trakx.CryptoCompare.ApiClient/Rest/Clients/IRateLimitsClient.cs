using System.Threading.Tasks;
using Trakx.CryptoCompare.ApiClient.Rest.Models.Responses;

namespace Trakx.CryptoCompare.ApiClient.Rest.Clients
{
    /// <summary>
    /// Interface of api client for cryptocompare api calls rate limits.
    /// </summary>
    public interface IRateLimitClient : IApiClient
    {
        /// <summary>
        /// Gets the rate limits left for you on the histo, price and news paths in the current hour..
        /// </summary>
        Task<RateLimitResponse> CurrentHourAsync();
    }
}
