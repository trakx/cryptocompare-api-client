﻿#pragma warning disable 8618
namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    /// <summary>
    /// A rate limit.
    /// </summary>
    /// <seealso cref="T:CryptoCompare.Responses.BaseApiResponse"/>
    public class RateLimitResponse : BaseApiResponse
    {
        /// <summary>
        /// Gets or sets the calls left.
        /// </summary>
        public Calls CallsLeft { get; set; }

        /// <summary>
        /// Gets or sets the calls made.
        /// </summary>
        public Calls CallsMade { get; set; }
    }
}
