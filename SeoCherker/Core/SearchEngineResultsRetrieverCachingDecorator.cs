using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data.Settings;

namespace OlegChibikov.SympliInterview.SeoChecker.Core
{
    public sealed class SearchEngineResultsRetrieverCachingDecorator : ISearchEngineResultsRetriever
    {
        readonly string _searchEngineName;
        readonly ISearchEngineResultsRetriever _underlyingRetriever;
        readonly IMemoryCache _memoryCache;
        readonly SemaphoreSlim _semaphore = new (1, 1);
        readonly IOptionsMonitor<SearchEngineRetrieverSettings> _optionsMonitor;

        public SearchEngineResultsRetrieverCachingDecorator(ISearchEngineResultsRetriever underlyingRetriever, IMemoryCache memoryCache, IOptionsMonitor<SearchEngineRetrieverSettings> optionsMonitor)
        {
            _underlyingRetriever = underlyingRetriever ?? throw new ArgumentNullException(nameof(underlyingRetriever));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            _searchEngineName = SearchEngine.GetName();
        }

        public SearchEngine SearchEngine => _underlyingRetriever.SearchEngine;

        public async Task<SearchEngineResults> GetSearchResultsAsync(string requestKeywords, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{_searchEngineName}_{requestKeywords}";
            var result = _memoryCache.Get<SearchEngineResults>(cacheKey);
            if (result != null)
            {
                return result;
            }

            try
            {
                await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

                // Cache may have been populated before entering semaphore, so we need to check one more time inside the critical section
                result = _memoryCache.Get<SearchEngineResults>(cacheKey);

                if (result != null)
                {
                    return result;
                }

                result = await _underlyingRetriever.GetSearchResultsAsync(requestKeywords, cancellationToken).ConfigureAwait(false);

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(_optionsMonitor.CurrentValue.CacheDuration);

                _memoryCache.Set(cacheKey, result, cacheEntryOptions);
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