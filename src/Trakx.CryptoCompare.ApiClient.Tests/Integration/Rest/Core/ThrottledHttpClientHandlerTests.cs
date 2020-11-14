﻿using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Trakx.CryptoCompare.ApiClient.Rest;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Core
{
    public class ThrottledHttpClientHandlerTests
    {
        [Fact]
        public async Task WaitsBetweenQueries()
        {
            var throttleDelayMs = 200;
            var queriesCount = 5;

            var configuration = new CryptoCompareApiConfiguration
            {
                ApiKey = Secrets.ApiKey,
                ThrottleDelayMs = throttleDelayMs
            };
            var options = Options.Create(configuration);
            var client = new CryptoCompareClient(options);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await Task.WhenAll(Enumerable.Repeat("start", queriesCount)
               .Select(async _ => await client.RateLimits.CurrentHourAsync()));


            stopWatch.Stop();

            var minExpectedElapsedTime = queriesCount * throttleDelayMs;
            Assert.True(stopWatch.ElapsedMilliseconds > minExpectedElapsedTime, 
                $"Elapsed time should have been greater than {minExpectedElapsedTime}ms, but was {stopWatch.ElapsedMilliseconds}ms.");
        }

    }
}
