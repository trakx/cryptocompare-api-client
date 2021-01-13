using System.Threading.Tasks;
using FluentAssertions;
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

        [Fact] public async Task CanCallWithLargeNumberOfFSymbols()
        {
            var symbols = new[]
            {
                "bix", "bz", "edo", "ftt", "ht", "kcs", "leo", "okb", "qash", "zb", "ast", "bnt", "dgtx", "hot", "idex",
                "knc", "lrc", "nec", "oax", "xin", "zrx", "lnd", "akro", "bcpt", "lba", "lend", "aave", "mkr", "nexo",
                "ppt", "rcn", "salt", "comp", "cel", "wbtc", "weth", "link", "seele", "bat", "bal", "omg", "usdc",
                "paxg", "uma", "yfi", "snx", "band", "mft", "hedg", "theta", "ren", "cvt", "chz", "rsr", "btc", "eth",
                "xrp", "bch", "bnb", "dot", "ltc", "ada", "eos", "waves", "xlm", "sushi", "zil", "xem", "uni", "rune",
                "srm", "cake", "luna"
            };
            var result = await CryptoCompareClient.Prices.MultipleSymbolsPriceAsync(symbols, new[] { "USD", "EUR" });
            result.Count.Should().Be(symbols.Length);
            Assert.NotNull(result);
        }
    }
}
