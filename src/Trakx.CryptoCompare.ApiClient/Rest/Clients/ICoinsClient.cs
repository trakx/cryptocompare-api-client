using System.Threading.Tasks;
using Trakx.CryptoCompare.ApiClient.Rest.Models.Responses;

namespace Trakx.CryptoCompare.ApiClient.Rest.Clients
{
    /// <summary>
    /// Coins api client. Gets general info for all the coins available on the website.
    /// </summary>
    public interface ICoinsClient : IApiClient
    {
        /// <summary>
        /// Returns all the coins that CryptoCompare has added to the website. 
        /// </summary>
        Task<CoinListResponse> ListAsync();
    }
}
