using System;
using Trakx.CryptoCompare.ApiClient.Rest.Helpers;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public readonly struct Sub : IEquatable<Sub>
    {
        public bool Equals(Sub other)
            => string.Equals(this.Exchange, other.Exchange)
            && string.Equals(this.BaseSymbol, other.BaseSymbol)
            && this.SubId == other.SubId
            && string.Equals(this.QuoteSymbol, other.QuoteSymbol);

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            return obj is Sub sub && this.Equals(sub);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Exchange, BaseSymbol, SubId, QuoteSymbol);
        }

        public Sub(string? exchange, string? fromSymbol, SubId subId, string? toSymbol)
        {
            Check.NotNullOrWhiteSpace(exchange, nameof(exchange));
            Check.NotNullOrWhiteSpace(fromSymbol, nameof(fromSymbol));
            Check.NotNullOrWhiteSpace(toSymbol, nameof(toSymbol));
            this.Exchange = exchange!;
            this.BaseSymbol = fromSymbol!;
            this.SubId = subId;
            this.QuoteSymbol = toSymbol!;
        }

        public string Exchange { get; }

        public string BaseSymbol { get; }

        public SubId SubId { get; }

        public string QuoteSymbol { get; }

        public override string ToString()
        {
            return $"{this.SubId:D}~{this.Exchange}~{this.BaseSymbol}~{this.QuoteSymbol}";
        }

        public static bool operator ==(Sub left, Sub right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Sub left, Sub right)
        {
            return !(left == right);
        }
    }
}
