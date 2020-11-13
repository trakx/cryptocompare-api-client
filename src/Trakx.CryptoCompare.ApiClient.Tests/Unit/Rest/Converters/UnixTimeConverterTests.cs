using System;
using System.Threading.Tasks;
using FluentAssertions;
using Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Unit.Rest.Converters
{
    public class UnixTimeConverterTests : CryptoCompareClientTestsBase
    {
        public UnixTimeConverterTests(CryptoCompareApiFixture apiFixture) : base(apiFixture)
        {
        }

        /// <summary>
        /// Should serialize candle data.
        /// </summary>
        /// <remarks>
        /// Test for https://github.com/joancaron/cryptocompare-api/issues/11
        /// </remarks>
        [Fact]
        public async Task ShouldSerializeCandleData()
        {
            var date = new DateTimeOffset(2018, 6, 15, 0, 0, 0, new TimeSpan(0, 0, 0));
            var hist = await CryptoCompareClient.History.HourlyAsync("BTC", "USD", 2, "Coinbase", date); 
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(hist);
            json.Should().NotBeNullOrEmpty();
        }
    }
}
