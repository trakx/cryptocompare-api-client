﻿using System;
using Newtonsoft.Json;
using Trakx.CryptoCompare.ApiClient.Rest.Core;
#pragma warning disable 8618

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    /// <summary>
    /// A base API response.
    /// CryptoCompare don't use status code for errors. They use a status reported into "Response" property
    /// </summary>
    public class BaseApiResponse
    {
        /// <summary>
        /// Gets or sets the errors summary.
        /// </summary>
        public string ErrorsSummary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a successful response.
        /// </summary>
        /// <value>
        /// True if this is a successful response, false if not.
        /// </value>
        public bool IsSuccessfulResponse => !string.Equals(
                                                this.Status,
                                                Constants.ResponseErrorStatus,
                                                StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets the full pathname of the resource called.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the response status.
        /// </summary>
        [JsonProperty("Response")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [JsonProperty("Message")]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the type of the status.
        /// </summary>
        [JsonProperty("Type")]
        public int StatusType { get; set; }
    }
}
