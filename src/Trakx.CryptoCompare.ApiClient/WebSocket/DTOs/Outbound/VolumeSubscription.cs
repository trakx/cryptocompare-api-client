namespace Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Outbound
{
    public abstract class VolumeSubscription : ICryptoCompareSubscription
    {
        protected VolumeSubscription(string type, string baseCurrency)
        {
            Type = type;
            BaseCurrency = baseCurrency.ToUpper();
        }
        
        /// <summary>
        /// A number used to identify the type of subscription.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Ticker of the base currency for which we want the updates.
        /// </summary>
        public string BaseCurrency { get; }

        #region Overrides of Object

        public override string ToString()
        {
            return $"{Type}~{BaseCurrency}";
        }

        #endregion
    }

    public sealed class FullVolumeSubscription : VolumeSubscription
    {
        internal const string TypeValue = "11";
        
        /// <inheritdoc />
        public FullVolumeSubscription(string baseCurrency) 
            : base(TypeValue, baseCurrency) {}
    }

    public sealed class FullTopTierVolumeSubscription : VolumeSubscription
    {
        internal const string TypeValue = "21";

        /// <inheritdoc />
        public FullTopTierVolumeSubscription(string baseCurrency)
            : base(TypeValue, baseCurrency) { }
    }
}