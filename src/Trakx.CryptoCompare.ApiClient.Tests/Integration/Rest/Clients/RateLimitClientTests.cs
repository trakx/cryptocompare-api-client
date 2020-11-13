using System.Threading.Tasks;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    public class RateLimitClientTests : CryptoCompareClientTestsBase
    {
        public RateLimitClientTests(CryptoCompareApiFixture apiFixture) : base(apiFixture)
        {
        }

        [Fact]
        public async Task CanCallRateLimitsCurrentHourEndpoint()
        {
            var result = await CryptoCompareClient.RateLimits.CurrentHourAsync();
            Assert.NotNull(result);
        }
    }
}
