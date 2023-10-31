using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trakx.CryptoCompare.ApiClient.Rest.Helpers;

namespace Trakx.CryptoCompare.ApiClient.Rest.Core
{
    public class ThottledHttpClientHandler : HttpClientHandler
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly int _millisecondsDelay;

        /// <summary>
        /// An <see cref="HttpClientHandler"></see> with a throttle to limit the maximum rate at which queries are sent.
        /// </summary>
        /// <param name="millisecondsDelay">The number of milliseconds to wait between calls to the base <see cref="HttpClientHandler.SendAsync"></see> method.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsDelay">millisecondsDelay</paramref> argument is less than or equal to 0.</exception>
        public ThottledHttpClientHandler(int millisecondsDelay)
        {
            if (millisecondsDelay <= 0) { throw new ArgumentOutOfRangeException(nameof(millisecondsDelay)); }

            this._millisecondsDelay = millisecondsDelay;
            this._semaphore = new SemaphoreSlim(1, 1);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Check.NotNull(request, nameof(request));

            await this._semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            finally
            {
                await Task.Delay(this._millisecondsDelay, cancellationToken).ConfigureAwait(false);
                this._semaphore.Release(1);
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._semaphore?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
