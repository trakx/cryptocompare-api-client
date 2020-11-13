using System.Threading.Tasks;
using Trakx.CryptoCompare.ApiClient.Rest.Models.Responses;

namespace Trakx.CryptoCompare.ApiClient.Rest.Clients
{
    /// <summary>
    /// Interface for exchanges api client.
    /// </summary>
    public interface IExchangesClient : IApiClient
    {
        /// <summary>
        /// all the exchanges that CryptoCompare has integrated with..
        /// </summary>
        Task<ExchangeListResponse> ListAsync();
    }
}
