﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.CryptoCompare.ApiClient.Rest;
using Trakx.CryptoCompare.ApiClient.WebSocket;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound;
using Trakx.Websocket.Interfaces;

namespace Trakx.CryptoCompare.ApiClient
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddCryptoCompareClient(
            this IServiceCollection services, CryptoCompareApiConfiguration configuration)
        {
            var options = Options.Create(configuration);
            services.AddSingleton(options);
            AddCommonDependencies(services);

            return services;
        }

        public static IServiceCollection AddCryptoCompareClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<CryptoCompareApiConfiguration>(configuration.GetSection(nameof(CryptoCompareApiConfiguration)));

            AddCommonDependencies(services);

            return services;
        }

        private static void AddCommonDependencies(IServiceCollection services)
        {
            services.AddSingleton<ICryptoCompareClient, CryptoCompareClient>();
            services
                .AddScoped<IClientWebsocketRedirectRegistry<InboundMessageBase, CryptoCompareWebsocketSubscription>,
                    CryptoCompareRedirectRegistry>();
        }
    }
}
