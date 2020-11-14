using System;
using System.Collections.Generic;

namespace Trakx.CryptoCompare.ApiClient.WebSocket
{
#nullable disable
    public partial class AllCoinsResponse
    {
        public string Response { get; set; }
        public string Message { get; set; }
        public Dictionary<string, CoinDetails> Data { get; set; }
        public Uri BaseImageUrl { get; set; }
        public Uri BaseLinkUrl { get; set; }
        public RateLimit RateLimit { get; set; }
        public bool HasWarning { get; set; }
        public long Type { get; set; }
    }

    public partial class CoinDetails
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public long ContentCreatedOn { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string CoinName { get; set; }
        public string FullName { get; set; }
        public string Algorithm { get; set; }
        public string ProofType { get; set; }
        public long FullyPremined { get; set; }
        public string TotalCoinSupply { get; set; }
        public string BuiltOn { get; set; }
        public string SmartContractAddress { get; set; }
        public string PreMinedValue { get; set; }
        public string TotalCoinsFreeFloat { get; set; }
        public long SortOrder { get; set; }
        public bool Sponsored { get; set; }
        public bool IsTrading { get; set; }
        public double? TotalCoinsMined { get; set; }
        public long? BlockNumber { get; set; }
        public double? NetHashesPerSecond { get; set; }
        public double? BlockReward { get; set; }
        public long? BlockTime { get; set; }
    }

    public partial class RateLimit
    {
    }
#nullable restore
}