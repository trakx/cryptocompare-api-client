using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Trakx.CryptoCompare.ApiClient.Rest.Clients;
using Trakx.CryptoCompare.ApiClient.Rest.Core;
using Trakx.CryptoCompare.ApiClient.Rest.Helpers;

namespace Trakx.CryptoCompare.ApiClient.Rest
{
    /// <summary>
    /// CryptoCompare api client.
    /// </summary>
    /// <seealso cref="T:Trakx.CryptoCompare.ApiClient.Rest.ICryptoCompareClient"/>
    public class CryptoCompareClient : ICryptoCompareClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the CryptoCompare.CryptoCompareClient class.
        /// </summary>
        /// <param name="httpClientHandler">Custom HTTP client handler. Can be used to define proxy settings.</param>
        /// <param name="apiConfiguration">Details of the Api Client configuration.</param>
        public CryptoCompareClient(HttpClientHandler httpClientHandler, IOptions<CryptoCompareApiConfiguration> apiConfiguration)
        {
            Check.NotNull(httpClientHandler, nameof(httpClientHandler));
            this._httpClient = new HttpClient(httpClientHandler, true);

            if (!string.IsNullOrWhiteSpace(apiConfiguration.Value.ApiKey))
            {
                this.SetApiKey(apiConfiguration.Value.ApiKey);
            }
        }

        /// <summary>
        /// Initializes a new instance of the CryptoCompare.CryptoCompareClient class.
        /// </summary>
        /// <param name="apiConfiguration">Details of the Api Client configuration.</param>
        public CryptoCompareClient(IOptions<CryptoCompareApiConfiguration> apiConfiguration)
            : this(
                apiConfiguration.Value.ThrottleDelayMs <= 0 
                    ? new HttpClientHandler() 
                    : new ThottledHttpClientHandler(apiConfiguration.Value.ThrottleDelayMs),
                apiConfiguration)
        {
        }

        public void SetApiKey(string apiKey)
        {
            Check.NotNullOrWhiteSpace(apiKey, nameof(apiKey));
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Apikey", apiKey);
        }

        /// <summary>
        /// Gets the client for coins related api endpoints.
        /// </summary>
        /// <seealso cref="P:Trakx.CryptoCompare.ApiClient.Rest.ICryptoCompareClient.Coins"/>
        public ICoinsClient Coins => new CoinsClient(this._httpClient);

        /// <summary>
        /// Gets the client for exchanges related api endpoints.
        /// </summary>
        /// <seealso cref="P:Trakx.CryptoCompare.ApiClient.Rest.ICryptoCompareClient.Exchanges"/>
        public IExchangesClient Exchanges => new ExchangesClient(this._httpClient);

        /// <summary>
        /// Gets the api client for market history.
        /// </summary>
        /// <seealso cref="P:Trakx.CryptoCompare.ApiClient.Rest.ICryptoCompareClient.History"/>
        public IHistoryClient History => new HistoryClient(this._httpClient);

        /// <summary>
        /// Gets the api client for "mining" endpoints.
        /// </summary>
        /// <value>
        /// The mining client.
        /// </value>
        /// <seealso cref="P:CryptoCompare.ICryptoCompareClient.MiningClient"/>
        public IMiningClient Mining => new MiningClient(this._httpClient);

        /// <summary>
        /// Gets the api client for news endpoints.
        /// </summary>
        /// <seealso cref="P:Trakx.CryptoCompare.ApiClient.Rest.ICryptoCompareClient.News"/>
        public INewsClient News => new NewsClient(this._httpClient);

        /// <summary>
        /// Gets the api client for cryptocurrency prices.
        /// </summary>
        /// <seealso cref="P:Trakx.CryptoCompare.ApiClient.Rest.ICryptoCompareClient.Prices"/>
        public IPricesClient Prices => new PriceClient(this._httpClient);

        /// <summary>
        /// Gets or sets the client for api calls rate limits.
        /// </summary>
        /// <seealso cref="P:Trakx.CryptoCompare.ApiClient.Rest.ICryptoCompareClient.RateLimits"/>
        public IRateLimitClient RateLimits => new RateLimitClient(this._httpClient);

        /// <summary>
        /// Gets the api client for "social" endpoints.
        /// </summary>
        /// <seealso cref="P:CryptoCompare.ICryptoCompareClient.Social"/>
        public ISocialStatsClient SocialStats => new SocialStatsClient(this._httpClient);

        /// <summary>
        /// The subs.
        /// </summary>
        public ISubsClient Subs => new SubsClient(this._httpClient);

        /// <summary>
        /// Gets the api client for "tops" endpoints.
        /// </summary>
        /// <seealso cref="P:Trakx.CryptoCompare.ApiClient.Rest.ICryptoCompareClient.Tops"/>
        public ITopListClient Tops => new TopListClient(this._httpClient);


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._httpClient.Dispose();
            }
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
