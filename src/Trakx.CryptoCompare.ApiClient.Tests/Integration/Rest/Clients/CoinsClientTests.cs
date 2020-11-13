using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    public class CoinsClientTests : CryptoCompareClientTestsBase
    {
        [Fact]
        public async Task CanCallListEndpoint()
        {
            var result = await CryptoCompareClient.Coins.ListAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallListEndpointAndRetrieveSmartContractAddresses()
        {
            var result = await CryptoCompareClient.Coins.ListAsync();
            Assert.NotNull(result);
            var foundSmartContractTokens = result.Coins.Any(
                c => c.Value.SmartContractAddress?.StartsWith("0x") ?? false);
            Assert.True(foundSmartContractTokens);
        }

        public CoinsClientTests(CryptoCompareApiFixture apiFixture) : base(apiFixture)
        {
        }
    }
}
