using System.Threading.Tasks;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    public class NewsClientTests : CryptoCompareClientTestsBase
    {
        public NewsClientTests(CryptoCompareApiFixture apiFixture) : base(apiFixture)
        { }

        [Fact]
        public async Task CanCallNewsListEndpoint()
        {
            var result = await CryptoCompareClient.News.News();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCallNewsProvidersListEndpoint()
        {
            var result = await CryptoCompareClient.News.NewsProviders();
            Assert.NotNull(result);
        }
    }
}
