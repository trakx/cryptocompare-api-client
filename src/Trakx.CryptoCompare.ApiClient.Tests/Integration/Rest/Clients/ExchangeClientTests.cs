using System.Threading.Tasks;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    public class ExchangeClientTests : CryptoCompareClientTestsBase
    {
        public ExchangeClientTests(CryptoCompareApiFixture apiFixture) : base(apiFixture)
        {
        }

        [Fact]
        public async Task CanCallExchangeListEndpoint()
        {
            var result = await CryptoCompareClient.Exchanges.ListAsync();
            Assert.NotNull(result);
        }
    }
}
