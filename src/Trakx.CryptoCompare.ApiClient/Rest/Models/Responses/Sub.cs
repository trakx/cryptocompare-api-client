using System;
using JetBrains.Annotations;
using Trakx.CryptoCompare.ApiClient.Rest.Helpers;

namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public readonly struct Sub : IEquatable<Sub>
    {
        public bool Equals(Sub other) => string.Equals(this.Exchange, other.Exchange)
                                            && string.Equals(this.BaseSymbol, other.BaseSymbol)
                                            && this.SubId == other.SubId
                                            && string.Equals(this.QuoteSymbol, other.QuoteSymbol);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Sub sub && this.Equals(sub);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Exchange.GetHashCode();
                hashCode = (hashCode * 397) ^ this.BaseSymbol.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)this.SubId;
                hashCode = (hashCode * 397) ^ this.QuoteSymbol.GetHashCode();
                return hashCode;
            }
        }

        public Sub([NotNull] string exchange, [NotNull] string fromSymbol, SubId subId, [NotNull] string toSymbol)
        {
            Check.NotNullOrWhiteSpace(exchange, nameof(exchange));
            Check.NotNullOrWhiteSpace(fromSymbol, nameof(fromSymbol));
            Check.NotNullOrWhiteSpace(toSymbol, nameof(toSymbol));
            this.Exchange = exchange;
            this.BaseSymbol = fromSymbol;
            this.SubId = subId;
            this.QuoteSymbol = toSymbol;
        }

        public string Exchange { get; }

        public string BaseSymbol { get; }

        public SubId SubId { get; }

        public string QuoteSymbol { get; }

        public override string ToString()
        {
            return $"{this.SubId:D}~{this.Exchange}~{this.BaseSymbol}~{this.QuoteSymbol}";
        }
    }
}
