﻿using System.Threading.Tasks;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    public class TopListClientTests : CryptoCompareClientTestsBase
    {
        [Fact]
        public async Task CanCallTopListByPairVolumeEndpoint()
        {
            var result = await CryptoCompareClient.Tops.ByPairVolumeAsync("BTC");
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallTopListExchangesFullDataByPairEndpoint()
        {
            var result = await CryptoCompareClient.Tops.ExchangesFullDataByPairAsync("BTC", "USD");
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallTopListExchangesVolumeDataByPairEndpoint()
        {
            var result = await CryptoCompareClient.Tops.ExchangesVolumeDataByPairAsync("BTC", "USD");
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallTopListTradingPairsEndpoint()
        {
            var result = await CryptoCompareClient.Tops.TradingPairsAsync("BTC");
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallTopListCoinFullDataByMarketCap()
        {
            var result = await CryptoCompareClient.Tops.CoinFullDataByMarketCap("EUR", 20);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallTopListCoinFullDataBy24HVolume()
        {
            var result = await CryptoCompareClient.Tops.CoinFullDataBy24HVolume("EUR", 20);
            Assert.NotNull(result);
        }

        public TopListClientTests(CryptoCompareApiFixture apiFixture) : base(apiFixture)
        {
        }
    }
}
