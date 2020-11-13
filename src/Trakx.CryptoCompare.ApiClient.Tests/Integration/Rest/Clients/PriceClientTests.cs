using System.Threading.Tasks;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    public class PriceClientTests : CryptoCompareClientTestsBase
    {
        public PriceClientTests(CryptoCompareApiFixture apiFixture) : base(apiFixture)
        {
        }

        [Fact]
        public async Task CanCallSingleSymbolPriceEndpoint()
        {
            var result = await CryptoCompareClient.Prices.SingleSymbolPriceAsync("BTC", new[] { "USD", "JPY", "EUR" });
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallMultipleSymbolFullDataEndpoint()
        {
            var result = await CryptoCompareClient.Prices.MultipleSymbolFullDataAsync(new[] { "BTC", "ETH" }, new[] { "USD", "EUR" });
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallMultipleSymbolPriceEndpoint()
        {
            var result = await CryptoCompareClient.Prices.MultipleSymbolsPriceAsync(new[] { "BTC", "ETH" }, new[] { "USD", "EUR" });
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallGenerateCustomAverageEndpoint()
        {
            var result = await CryptoCompareClient.Prices.GenerateCustomAverageAsync("BTC", "USD", new[] { "Kraken" });
            Assert.NotNull(result);
        }
    }
}
