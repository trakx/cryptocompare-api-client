using System.Threading.Tasks;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    public class SocialStatsClientTests : CryptoCompareClientTestsBase
    {
        public SocialStatsClientTests(CryptoCompareApiFixture apiFixture) : base(apiFixture)
        {
        }

        [Fact]
        public async Task CanCallSocialStatsEndpoint()
        {
            var result = await CryptoCompareClient.SocialStats.StatsAsync(1182);
            Assert.NotNull(result);
        }
    }
}
