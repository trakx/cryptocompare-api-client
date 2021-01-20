using Trakx.Utils.Attributes;
using Trakx.Utils.Testing;

namespace Trakx.CryptoCompare.ApiClient.Tests
{
    public record Secrets : SecretsBase
    {
        [SecretEnvironmentVariable(nameof(CryptoCompareApiConfiguration), nameof(CryptoCompareApiConfiguration.ApiKey))]
        public string ApiKey { get; init; }
    }
}
