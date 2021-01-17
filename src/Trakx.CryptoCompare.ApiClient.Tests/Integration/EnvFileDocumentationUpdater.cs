using Trakx.Utils.Testing;
using Xunit.Abstractions;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration
{
    public class EnvFileDocumentationUpdater : EnvFileDocumentationUpdaterBase
    {
        /// <inheritdoc />
        public EnvFileDocumentationUpdater(ITestOutputHelper output, IReadmeEditor? editor = null) : base(output, editor)
        {
        }
    }
}