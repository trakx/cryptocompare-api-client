using System;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound;

namespace Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Outbound
{
    public abstract class ExchangeBaseQuoteSubscription : VolumeSubscription
    {
        protected ExchangeBaseQuoteSubscription(string type, string exchange, string baseCurrency, string quoteCurrency) 
            : base(type, baseCurrency)
        {
            Exchange = exchange;
            QuoteCurrency = quoteCurrency.ToUpper();
        }

        /// <summary>
        /// The exchange for which we want the data. This can
        /// also be CCCAGG for CryptoCompare's own Aggregate Indice.
        /// </summary>
        public string Exchange { get; }

        /// <summary>
        /// Ticker of the currency in which we want the data to be quoted.
        /// </summary>
        public string QuoteCurrency { get; }

        #region Overrides of Object

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Type}~{Exchange}~{BaseCurrency}~{QuoteCurrency}";
        }

        #endregion
    }

    public sealed class AggregateIndexSubscription : ExchangeBaseQuoteSubscription
    {
        internal const string TypeValue = AggregateIndex.TypeValue;
        
        /// <inheritdoc />
        public AggregateIndexSubscription(string baseCurrency, string quoteCurrency)
            : base(AggregateIndex.TypeValue, "CCCAGG", baseCurrency, quoteCurrency) { }
    }

    public sealed class TradeSubscription : ExchangeBaseQuoteSubscription
    {
        internal const string TypeValue = Trade.TypeValue;

        /// <inheritdoc />
        public TradeSubscription(string exchange, string baseCurrency, string quoteCurrency)
            : base(Trade.TypeValue, exchange, baseCurrency, quoteCurrency) { }
    }

    public sealed class TickerSubscription : ExchangeBaseQuoteSubscription
    {
        internal const string TypeValue = Ticker.TypeValue;

        /// <inheritdoc />
        public TickerSubscription(string exchange, string baseCurrency, string quoteCurrency)
            : base(Ticker.TypeValue, exchange, baseCurrency, quoteCurrency) { }
    }

    public sealed class OhlcSubscription : ExchangeBaseQuoteSubscription
    {
        internal const string TypeValue = Ohlc.TypeValue;

        public string Peridocity { get; }

        /// <inheritdoc />
        public OhlcSubscription(string exchange, string baseCurrency, string quoteCurrency, TimeSpan peridocity = default)
            : base("24", exchange, baseCurrency, quoteCurrency)
        {
            Peridocity = peridocity.TotalMilliseconds >= TimeSpan.FromDays(1).TotalSeconds
                ? "D"
                : peridocity.TotalMilliseconds >= TimeSpan.FromHours(1).TotalSeconds
                    ? "H"
                    : "m";
        }

        #region Overrides of ExchangeBaseQuoteSubscription

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{base.ToString()}~{Peridocity}";
        }

        #endregion
    }
}