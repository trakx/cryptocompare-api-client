using System;
using Microsoft.Extensions.DependencyInjection;
using Trakx.Common.Testing.Configuration;
using Trakx.CryptoCompare.ApiClient.Rest;
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
            CryptoCompareApiConfiguration configuration = LoadConfiguration();

            var services = new ServiceCollection();
            services.AddCryptoCompareClient(configuration);
            var provider = services.BuildServiceProvider();
            CryptoCompareClient = provider.GetRequiredService<ICryptoCompareClient>();
        }

        public static CryptoCompareApiConfiguration LoadConfiguration()
        {
            return AwsConfigurationHelper.GetConfigurationFromAws<CryptoCompareApiConfiguration>()
                ?? throw new InvalidOperationException("Unable to load configuration from AWS");
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
