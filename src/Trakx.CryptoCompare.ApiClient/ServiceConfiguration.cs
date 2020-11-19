using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Trakx.CryptoCompare.ApiClient.Rest;
using Trakx.CryptoCompare.ApiClient.WebSocket;

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
            Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().CreateLogger();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();

            services.AddSingleton(Log.Logger);

            services.AddOptions();
            services.Configure<CryptoCompareApiConfiguration>(configuration.GetSection(nameof(CryptoCompareApiConfiguration)));

            AddCommonDependencies(services);

            return services;
        }

        private static void AddCommonDependencies(IServiceCollection services)
        {
            services.AddSingleton<ICryptoCompareClient, CryptoCompareClient>();
            services.AddTransient<IClientWebsocket, WrappedClientWebsocket>();
            services.AddTransient<IWebSocketStreamer, WebSocketStreamer>();
            services.AddSingleton<ICryptoCompareWebSocketClient, CryptoCompareWebSocketClient>();
            services.AddSingleton<WrappedClientWebsocket>();
        }
    }
}
