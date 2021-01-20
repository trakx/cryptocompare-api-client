using System;
using Microsoft.Extensions.Options;
using Trakx.CryptoCompare.ApiClient.Rest;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients
{
    [Collection(nameof(ApiTestCollection))]
    public class CryptoCompareClientTestsBase
    {
        protected CryptoCompareClient CryptoCompareClient { get; }
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
        public CryptoCompareClient CryptoCompareClient { get; }
        public CryptoCompareApiFixture()
        {
            var configuration = new CryptoCompareApiConfiguration { ApiKey = new Secrets().ApiKey };
            var options = Options.Create(configuration)!;
            CryptoCompareClient = new CryptoCompareClient(options);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            this.CryptoCompareClient.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}