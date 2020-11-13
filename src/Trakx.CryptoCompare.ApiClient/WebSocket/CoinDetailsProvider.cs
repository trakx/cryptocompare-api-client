using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Trakx.CryptoCompare.ApiClient.WebSocket
{
    public class CoinDetailsProvider
    {
        public static IReadOnlyDictionary<string, CoinDetails> CoinDetailsBySymbol { get; } =
            ReadCoinDetailsFromResource().GetAwaiter().GetResult().Data;

        private static async Task<AllCoinsResponse> ReadCoinDetailsFromResource()
        {
            var assembly = typeof(CoinDetailsProvider).Assembly;
            await using var stream = assembly.GetManifestResourceStream(
                    $"{typeof(CoinDetailsProvider).Namespace}.coinDetails.json");
            var response = await JsonSerializer.DeserializeAsync<AllCoinsResponse>(stream);
            return response;
        }
        public List<string> GetAllErc20Symbols()
        {
            var smartContractCoins = CoinDetailsBySymbol.Values.Where(c =>
                c.SmartContractAddress.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase));

            return smartContractCoins.Select(c => c.Symbol).ToList();
        }
    }
}