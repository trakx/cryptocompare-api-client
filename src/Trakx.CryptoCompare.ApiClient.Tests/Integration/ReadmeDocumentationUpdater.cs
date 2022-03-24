using Trakx.Utils.Testing.ReadmeUpdater;
using Xunit.Abstractions;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration
{
    public class EnvFileDocumentationUpdater : ReadmeDocumentationUpdaterBase
    {
        /// <inheritdoc />
        public EnvFileDocumentationUpdater(ITestOutputHelper output) : base(output)
        {
        }
    }
}
