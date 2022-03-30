using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.CryptoCompare.ApiClient.Rest;

namespace Trakx.CryptoCompare.ApiClient
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddCryptoCompareClient(
            this IServiceCollection services, CryptoCompareApiConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddSingleton<ICryptoCompareClient, CryptoCompareClient>();
            return services;
        }

        public static IServiceCollection AddCryptoCompareClient(this IServiceCollection services, IConfiguration configuration)
        {
            var apiConfiguration = configuration.GetSection(nameof(CryptoCompareApiConfiguration))
                .Get<CryptoCompareApiConfiguration>();
            return AddCryptoCompareClient(services, apiConfiguration);
        }
    }
}
