using Microsoft.Extensions.DependencyInjection;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCryptoCompareWebsocketHandler(this IServiceCollection services, CryptoCompareWebsocketConfiguration config)
    {
        services.AddSingleton<ICryptoCompareWebsocketHandler, CryptoCompareWebsocketHandler>();
        services.AddSingleton(config);
    }
}
