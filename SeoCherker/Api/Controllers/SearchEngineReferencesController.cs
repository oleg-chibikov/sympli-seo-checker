using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data;

namespace OlegChibikov.SympliInterview.SeoChecker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchEngineReferencesController : ControllerBase
    {
        readonly IEnumerable<ISearchEngineResultsRetriever> _searchEngineResultsRetrievers;
        readonly IReferenceMatcher _referenceMatcher;

        public SearchEngineReferencesController(IEnumerable<ISearchEngineResultsRetriever> searchEngineResultsRetrievers, IReferenceMatcher referenceMatcher)
        {
            _searchEngineResultsRetrievers = searchEngineResultsRetrievers ?? throw new ArgumentNullException(nameof(searchEngineResultsRetrievers));
            _referenceMatcher = referenceMatcher ?? throw new ArgumentNullException(nameof(referenceMatcher));
        }

        [HttpGet("{requestKeywords}/{reference}")]
        public async Task<IEnumerable<SearchEngineReferences>> GetAsync(string requestKeywords = "e-settlements", string reference = "www.sympli.com.au", CancellationToken cancellationToken = default)
        {
            _ = requestKeywords ?? throw new ArgumentNullException(nameof(requestKeywords));
            _ = reference ?? throw new ArgumentNullException(nameof(reference));

            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new BusinessException("Empty reference");
            }

            if (string.IsNullOrWhiteSpace(requestKeywords))
            {
                throw new BusinessException("Empty requestKeywords");
            }

            var tasks = _searchEngineResultsRetrievers.Select(
                async searchEngineResultsRetriever =>
                {
                    var searchResults = await searchEngineResultsRetriever.GetSearchResultsAsync(requestKeywords, cancellationToken).ConfigureAwait(false);
                    return new SearchEngineReferences(searchResults.SearchEngine, _referenceMatcher.MatchReferenceToResults(reference, searchResults.WebsiteNames), searchResults.HasError);
                });

            return await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}