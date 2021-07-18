using System;
using Microsoft.Extensions.Caching.Memory;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;

namespace OlegChibikov.SympliInterview.SeoChecker.Core
{
    public sealed class MemoryCacheAdapter : ICache
    {
        readonly IMemoryCache _memoryCache;

        public MemoryCacheAdapter(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public TItem? Get<TItem>(string key)
        {
            return _memoryCache.Get<TItem>(key);
        }

        public void Set<TItem>(string key, TItem value, TimeSpan absoluteExpirationRelativeToNow)
        {
            _memoryCache.Set(key, value, absoluteExpirationRelativeToNow);
        }
    }
}