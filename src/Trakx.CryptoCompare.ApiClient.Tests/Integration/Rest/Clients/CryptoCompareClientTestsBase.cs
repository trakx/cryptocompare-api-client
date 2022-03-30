using System;
using Microsoft.Extensions.DependencyInjection;
using Trakx.CryptoCompare.ApiClient.Rest;
using Trakx.Utils.Testing;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    [Collection(nameof(ApiTestCollection))]
    public class CryptoCompareClientTestsBase
    {
        protected ICryptoCompareClient CryptoCompareClient { get; }
        public CryptoCompareClientTestsBase(CryptoCompareApiFixture apiFixture)
        {
            CryptoCompareClient = apiFixture.CryptoCompareClient;
        }
    }

    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<CryptoCompareApiFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class CryptoCompareApiFixture : IDisposable
    {
        public ICryptoCompareClient CryptoCompareClient { get; }
        public CryptoCompareApiFixture()
        {
            var configuration = ConfigurationHelper.GetConfigurationFromEnv<CryptoCompareApiConfiguration>();
            var services = new ServiceCollection();
            services.AddCryptoCompareClient(configuration);
            var provider = services.BuildServiceProvider();
            CryptoCompareClient = provider.GetRequiredService<ICryptoCompareClient>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            CryptoCompareClient.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
