﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Trakx.CryptoCompare.ApiClient.Rest.Helpers
{
    /// <summary>
    /// Null checking utilities.
    /// </summary>
    [DebuggerStepThrough]
    internal static class Check
    {
        /// <summary>
        /// Checks null enmerable.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or
        /// illegal values.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter. This cannot be null.</param>
        [ContractAnnotation("value:null => halt")]
        public static IEnumerable<T> NotEmpty<T>(
            IEnumerable<T> value,
            [InvokerParameterName][NotNull] string parameterName)
        {
            NotNull(value, parameterName);

            if (!value.Any())
            {
                NotNullOrWhiteSpace(parameterName, nameof(parameterName));
                throw new ArgumentException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Checks null arguments.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter. This cannot be null.</param>
        /// <returns>
        /// Checked object.
        /// </returns>
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(T value, [InvokerParameterName][NotNull] string parameterName)
        {
            if (value is null)
            {
                NotNullOrWhiteSpace(parameterName, nameof(parameterName));
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        /// <summary>
        /// Checks null or white space string arguments.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter. This cannot be null.</param>
        /// <returns>
        /// Checked object.
        /// </returns>
        [ContractAnnotation("value:null => halt")]
        public static string NotNullOrWhiteSpace(string? value, [InvokerParameterName][NotNull] string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                NotNullOrWhiteSpace(parameterName, nameof(parameterName));
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }
    }
}
