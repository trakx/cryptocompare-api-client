using Microsoft.AspNetCore.Builder;

namespace CryptoCompareServer.Middleware
{
    public static class CryptoCompareServerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCryptoCompareServer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CryptoCompareServerMiddleware>();
        }
    }
}