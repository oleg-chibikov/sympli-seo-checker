using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data.Settings;

namespace OlegChibikov.SympliInterview.SeoChecker.Core
{
    public abstract class CacheWrapper
    {
        readonly ICache _cache;
        readonly SemaphoreSlim _semaphore = new (1, 1);
        readonly IOptionsMonitor<CachingSettings> _optionsMonitor;

        protected CacheWrapper(IOptionsMonitor<CachingSettings> optionsMonitor, ICache cache)
        {
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        protected async Task<T> GetFromCacheAsync<T>(string cacheKey, Func<CancellationToken, Task<T>> getDataAsync, CancellationToken cancellationToken = default)
        {
            _ = getDataAsync ?? throw new ArgumentNullException(nameof(getDataAsync));

            var result = _cache.Get<T>(cacheKey);
            if (result != null)
            {
                return result;
            }

            try
            {
                await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

                // Cache may have been populated before entering semaphore, so we need to check one more time inside the critical section
                result = _cache.Get<T>(cacheKey);

                if (result != null)
                {
                    return result;
                }

                result = await getDataAsync(cancellationToken).ConfigureAwait(false);

                _cache.Set(cacheKey, result, _optionsMonitor.CurrentValue.CacheDuration);
                return result;
            }
            finally
            {
                // semaphore should be released regardless of any exceptions
                _semaphore.Release();
            }
        }
    }
}