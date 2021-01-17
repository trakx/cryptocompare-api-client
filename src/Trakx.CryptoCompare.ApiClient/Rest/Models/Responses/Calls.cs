#pragma warning disable 8618
namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    /// <summary>
    /// Api calls.
    /// </summary>
public class Calls
    {
        /// <summary>
        /// Calls to history apis.
        /// </summary>
        public int Histo { get; set; }

        /// <summary>
        /// Calls to news api.
        /// </summary>
        public int News { get; set; }

        /// <summary>
        /// Calls to price apis.
        /// </summary>
        public int Price { get; set; }
    }
}
