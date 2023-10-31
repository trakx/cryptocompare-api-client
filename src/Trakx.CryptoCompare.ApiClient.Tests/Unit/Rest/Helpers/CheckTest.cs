using System;
using FluentAssertions;
using Trakx.CryptoCompare.ApiClient.Rest.Helpers;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Unit.Rest.Helpers
{
    public class CheckTest
    {
        internal const string Blah = nameof(Blah);

        /// <summary>
        /// NotNullOrWhiteSpace should not throw ArgumentNullException when string is not null.
        /// </summary>
        [Fact]
        public void NotNullOrWhiteSpaceShouldNotThrowArgumentNullExceptionWhenStringIsNotNull()
        {
            Check.NotNullOrWhiteSpace(Blah, Blah);
        }

        /// <summary>
        /// NotNullOrWhiteSpace should throw ArgumentNullException when string is null or empty or whitespace.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void NotNullOrWhiteSpaceShouldThrowArgumentNullExceptionWhenStringIsNullOrEmptyOrWhitespace(string value)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Check.NotNullOrWhiteSpace(value, Blah));
            exception.ParamName.Should().Be(Blah);
        }

        /// <summary>
        /// NotNull should not throw ArgumentNullException when object is not null.
        /// </summary>
        [Fact]
        public void NotNullShouldNotThrowArgumentNullExceptionWhenObjectIsNotNull()
        {
            Check.NotNull<int?>(1, Blah);
        }

        /// <summary>
        /// NotNull should throw ArgumentNullException when object is null.
        /// </summary>
        [Fact]
        public void NotNullShouldThrowArgumentNullExceptionWhenObjectIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Check.NotNull<int?>(null, Blah));
            exception.ParamName.Should().Be(Blah);
        }
    }
}
