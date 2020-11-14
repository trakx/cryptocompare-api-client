namespace Trakx.CryptoCompare.ApiClient.Rest.Models.Responses
{
    public class MiningContract : MiningData
    {
        public string? ContractLength { get; set; }

        public string? FeePercentage { get; set; }

        public string? FeeValue { get; set; }

        public string? FeeValueCurrency { get; set; }
    }
}
