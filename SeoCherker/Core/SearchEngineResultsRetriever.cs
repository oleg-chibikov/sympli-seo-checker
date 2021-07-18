using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data.Settings;
using IQueryProvider = OlegChibikov.SympliInterview.SeoChecker.Contracts.IQueryProvider;

namespace OlegChibikov.SympliInterview.SeoChecker.Core
{
    public sealed class SearchEngineResultsRetriever : ISearchEngineResultsRetriever
    {
        readonly string _searchEngineName;
        readonly IHttpClientFactory _httpClientFactory;
        readonly ISearchEngineResultsParser _searchEngineResultsParser;
        readonly IQueryProvider _queryProvider;
        readonly IOptionsMonitor<SearchEngineRetrieverSettings> _optionsMonitor;

        public SearchEngineResultsRetriever(
            IHttpClientFactory httpClientFactory,
            IOptionsMonitor<SearchEngineRetrieverSettings> optionsMonitor,
            Func<SearchEngine, ISearchEngineResultsParser> searchEngineResultsParserResolver,
            Func<SearchEngine, IQueryProvider> queryProviderResolver,
            SearchEngine searchEngine)
        {
            _ = queryProviderResolver ?? throw new ArgumentNullException(nameof(queryProviderResolver));
            _ = searchEngineResultsParserResolver ?? throw new ArgumentNullException(nameof(searchEngineResultsParserResolver));

            SearchEngine = searchEngine;
            _searchEngineName = searchEngine.GetName();
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            _searchEngineResultsParser = searchEngineResultsParserResolver(searchEngine);
            _queryProvider = queryProviderResolver(searchEngine);
        }

        public SearchEngine SearchEngine { get; }

        public async Task<SearchEngineResults> GetSearchResultsAsync(string requestKeywords, CancellationToken cancellationToken = default)
        {
            _ = requestKeywords ?? throw new ArgumentNullException(nameof(requestKeywords));

            if (string.IsNullOrWhiteSpace(requestKeywords))
            {
                throw new InvalidOperationException("Empty requestKeywords");
            }

            var allParsedResults = new List<string>();
            var uri = new Uri(_queryProvider.GetRelativeUriPart(HttpUtility.HtmlEncode(requestKeywords)), UriKind.Relative);

            // we are recreating the httpClient for each request (except of getting several pages of the same request) instead of keeping it for the lifetime of the application.
            using var httpClient = _httpClientFactory.CreateClient(_searchEngineName);

            while (true)
            {
                using var response = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();
                var rawResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                var parsedResults = _searchEngineResultsParser.ParseResults(rawResponse).ToArray();
                allParsedResults.AddRange(parsedResults);

                // We keep switching the pages until there are no more pages or we have collected the required number of results.
                if (allParsedResults.Count >= _optionsMonitor.CurrentValue.RequiredResults)
                {
                    break;
                }

                // Instead of simply calculating the data for next page we are getting the uri from the pager on the current page returned by the Search Engine.
                // This is done because sometimes Search Engines don't follow the normal paging rules
                // or the parser treats page results slightly differently than the Search Engine (e.g. it might exclude the Videos or Images sections, while they are treated as results by the engine in terms of Paging).
                var nextPageUri = _searchEngineResultsParser.ParseNextPageUri(rawResponse);

                if (nextPageUri == null)
                {
                    break;
                }

                uri = nextPageUri;
            }

            return new SearchEngineResults(_searchEngineName, allParsedResults);
        }
    }
}