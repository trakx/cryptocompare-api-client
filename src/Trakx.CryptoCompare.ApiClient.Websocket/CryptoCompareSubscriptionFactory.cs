﻿using System.Text.Json;
using Trakx.CryptoCompare.ApiClient.Websocket.Model;
using Trakx.Websocket.Model;

namespace Trakx.CryptoCompare.ApiClient.Websocket
{
    public static class CryptoCompareSubscriptionFactory
    {
        public static TopicSubscription GetTopicSubscription(SubscribeActions action, string[] subs)
        {
            return new TopicSubscription(JsonSerializer.Serialize(new CryptoCompareSubscription
            {
                Action = action,
                Subs = subs
            }));
        }

        public static string GetTopOfOrderBookSubscriptionStr(string exchange, string baseCurrency, string quoteCurrency) => $"30~{exchange}~{baseCurrency.ToUpper()}-{quoteCurrency.ToUpper()}";
        public static string GetOHLCCandlesSubscriptionStr(string source, string baseCurrency, string quoteCurrency) => $"24~{source}~{baseCurrency.ToUpper()}-{quoteCurrency.ToUpper()}";
        public static string GetFullTopTierVolumeSubscriptionStr(string baseCurrency) => $"21~{baseCurrency.ToUpper()}";        
        public static string GetFullVolumeSubscriptionStr(string baseCurrency) => $"11~{baseCurrency.ToUpper()}";
        public static string GetOrderBookL2SubscriptionStr(string exchange, string baseCurrency, string quoteCurrency) => $"8~{exchange}~{baseCurrency.ToUpper()}-{quoteCurrency.ToUpper()}";
        public static string GetCCCAGGSubscriptionStr(string baseCurrency, string quoteCurrency) => $"5~CCCAGG~{baseCurrency.ToUpper()}-{quoteCurrency.ToUpper()}";        
        public static string GetTickerSubscriptionStr(string exchange, string baseCurrency, string quoteCurrency) => $"2~{exchange}-{baseCurrency.ToUpper()}-{quoteCurrency.ToUpper()}";
        public static string GetTradeSubscriptionStr(string exchange, string baseCurrency, string quoteCurrency) => $"0~{exchange}-{baseCurrency.ToUpper()}-{quoteCurrency.ToUpper()}";
    }
}
