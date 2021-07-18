using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data.Settings;

namespace OlegChibikov.SympliInterview.SeoChecker.Core
{
    public sealed class SearchEngineResultsRetrieverCachingDecorator : CacheWrapper, ISearchEngineResultsRetriever
    {
        readonly string _searchEngineName;
        readonly ISearchEngineResultsRetriever _underlyingRetriever;

        public SearchEngineResultsRetrieverCachingDecorator(IOptionsMonitor<CachingSettings> optionsMonitor, ICache cache, ISearchEngineResultsRetriever underlyingRetriever) : base(
            optionsMonitor,
            cache)
        {
            _underlyingRetriever = underlyingRetriever ?? throw new ArgumentNullException(nameof(underlyingRetriever));
            _searchEngineName = underlyingRetriever.SearchEngine.GetName();
        }

        public SearchEngine SearchEngine => _underlyingRetriever.SearchEngine;

        public async Task<SearchEngineResults> GetSearchResultsAsync(string requestKeywords, CancellationToken cancellationToken = default) =>
            await GetFromCacheAsync(
                    $"{_searchEngineName}_{requestKeywords}",
                    async ct => await _underlyingRetriever.GetSearchResultsAsync(requestKeywords, ct).ConfigureAwait(false),
                    cancellationToken)
                .ConfigureAwait(false);
    }
}