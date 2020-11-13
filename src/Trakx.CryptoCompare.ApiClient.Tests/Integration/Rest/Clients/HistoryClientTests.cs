using System;
using System.Threading.Tasks;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    public class HistoryClientTests : CryptoCompareClientTestsBase
    {
        public HistoryClientTests(CryptoCompareApiFixture apiFixture) : base(apiFixture)
        {
        }

        [Fact]
        public async Task CanCallDayAveragePriceEndpoint()
        {
            var result = await CryptoCompareClient.History.DayAveragePriceAsync("BTC", "USD");
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallExchangeDailyEndpoint()
        {
            var result = await CryptoCompareClient.History.ExchangeDailyAsync("BTC");
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallExchangeHourlyEndpoint()
        {
            var result = await CryptoCompareClient.History.ExchangeHourlyAsync("BTC");
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallHistoricalDailyEndpoint()
        {
            var result = await CryptoCompareClient.History.DailyAsync("BTC", "USD", 10, allData: true);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallHistoricalForTimestampEndpoint()
        {
            var result = await CryptoCompareClient.History.HistoricalForTimestampAsync(
                             "BTC",
                             new[] { "USD" },
                             DateTimeOffset.Now.AddDays(-1));
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallHistoricalHourlyEndpoint()
        {
            var result = await CryptoCompareClient.History.HourlyAsync("BTC", "USD", 10);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallHistoricalMinutelyEndpoint()
        {
            var result = await CryptoCompareClient.History.MinutelyAsync("BTC", "USD", 10);
            Assert.NotNull(result);
        }
    }
}
