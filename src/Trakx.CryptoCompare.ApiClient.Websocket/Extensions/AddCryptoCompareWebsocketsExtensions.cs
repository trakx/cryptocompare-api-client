using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trakx.Common.Configuration;
using Trakx.Websocket;
using Trakx.Websocket.Interfaces;
using Trakx.Websocket.Model;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Extensions;

public static class AddCryptoCompareWebsocketsExtensions
{
    public static void AddCryptoCompareWebsockets(
        this IServiceCollection services,
        CryptoCompareApiConfiguration apiConfiguration,
        WebsocketConfiguration webSocketConfiguration)
    {
        services.AddSingleton<ICryptoCompareWebsocketHandler, CryptoCompareWebsocketHandler>();
        services.AddSingleton<IClientWebsocketFactory, ClientWebsocketFactory>();
        services.AddSingleton(apiConfiguration);
        services.AddSingleton(webSocketConfiguration);
    }

    public static void AddCryptoCompareWebsockets(this IServiceCollection services, IConfiguration config)
    {
        var apiConfiguration = config.GetConfiguration<CryptoCompareApiConfiguration>();
        var webSocketConfig = config.GetConfiguration<WebsocketConfiguration>();
        AddCryptoCompareWebsockets(services, apiConfiguration, webSocketConfig);
    }
}
