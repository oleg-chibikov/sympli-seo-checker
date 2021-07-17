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
        readonly IEnumerable<ISearchEngineResultsParser> _searchEngineResultsParsers;

        public SearchEngineReferencesController(IEnumerable<ISearchEngineResultsParser> searchEngineResultsParsers)
        {
            _searchEngineResultsParsers = searchEngineResultsParsers ?? throw new ArgumentNullException(nameof(searchEngineResultsParsers));
        }

        [HttpGet("{requestKeywords}/{reference}")]
        public async Task<IEnumerable<SearchEngineReferences>> GetAsync(string requestKeywords = "e-settlements", string reference = "www.sympli.com.au", CancellationToken cancellationToken = default)
        {
            _ = reference ?? throw new ArgumentNullException(nameof(reference));
            _ = requestKeywords ?? throw new ArgumentNullException(nameof(requestKeywords));

            // TODO: Caching
            var tasks = _searchEngineResultsParsers.Select(x => x.GetReferencesAsync(requestKeywords, reference, cancellationToken));
            return await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}