
using Microsoft.Extensions.DependencyInjection;
using Trakx.CryptoCompare.ApiClient.WebSocket;
using CryptoCompareClient = Trakx.CryptoCompare.ApiClient.Rest.CryptoCompareClient;
using ICryptoCompareClient = Trakx.CryptoCompare.ApiClient.Rest.ICryptoCompareClient;

namespace Trakx.CryptoCompare.ApiClient
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddCryptoCompareClient(this IServiceCollection services)
        {
            services.AddSingleton<IApiDetailsProvider, ApiDetailsProvider>();
            services.AddSingleton<ICryptoCompareClient, CryptoCompareClient>(provider =>
            {
                var apiKey = provider.GetService<IApiDetailsProvider>().ApiKey;
                return new CryptoCompareClient(apiKey);
            });


            services.AddTransient<IClientWebsocket, WrappedClientWebsocket>();
            services.AddTransient<IWebSocketStreamer, WebSocketStreamer>();
            services.AddSingleton<ICryptoCompareWebSocketClient, CryptoCompareWebSocketClient>();
            services.AddSingleton<WrappedClientWebsocket>();
            
            return services;
        }
    }
}
