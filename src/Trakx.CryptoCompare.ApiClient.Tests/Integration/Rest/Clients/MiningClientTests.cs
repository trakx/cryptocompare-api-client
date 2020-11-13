using System.Threading.Tasks;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    public class MiningClientTests : CryptoCompareClientTestsBase
    {
        public MiningClientTests(CryptoCompareApiFixture apiFixture) : base(apiFixture)
        {
        }

        [Fact]
        public async Task CanCallMiningContractsEndpoint()
        {
            var result = await CryptoCompareClient.Mining.ContractsAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallMiningEquipmentsEndpoint()
        {
            var result = await CryptoCompareClient.Mining.EquipmentsAsync();
            Assert.NotNull(result);
        }
    }
}
