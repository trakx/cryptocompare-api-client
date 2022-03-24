using Microsoft.Extensions.DependencyInjection;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Extensions;

public static class AddCryptoCompareWebsocketExtensions
{
    public static void AddCryptoCompareWebsocketHandler(this IServiceCollection services, CryptoCompareApiConfiguration config)
    {
        services.AddSingleton<ICryptoCompareWebsocketHandler, CryptoCompareWebsocketHandler>();
        services.AddSingleton(config);
    }
}
