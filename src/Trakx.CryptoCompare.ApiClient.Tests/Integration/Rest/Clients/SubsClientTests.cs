using System.Threading.Tasks;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    public class SubsClientTests : CryptoCompareClientTestsBase
    {
        public SubsClientTests(CryptoCompareApiFixture apiFixture) : base(apiFixture)
        {
        }

        [Fact]
        public async Task CanCallSocialStatsEndpoint()
        {
            var result = await CryptoCompareClient.Subs.ListAsync("BTC", new[] { "USD" });
            Assert.NotNull(result);
        }
    }
}
