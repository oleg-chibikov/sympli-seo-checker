using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data;

namespace OlegChibikov.SympliInterview.SeoChecker.Core
{
    public class GoogleResultsParser : ISearchEngineResultsParser, IDisposable
    {
        readonly HttpClient _httpClient;

        public GoogleResultsParser(IHttpClientFactory httpClientFactory)
        {
            // httpClient instance can be kept for the lifetime of the class. It's efficient provided that we query the same web address with this instance of httpClient
            _httpClient = httpClientFactory.CreateClient(SearchEngine.Google.GetName());
        }

        public SearchEngine SearchEngine => SearchEngine.Google;

        public Task<SearchEngineReferences> GetReferencesAsync(string requestKeywords, string reference, CancellationToken cancellationToken = default)
        {
            _ = reference ?? throw new ArgumentNullException(nameof(reference));
            _ = requestKeywords ?? throw new ArgumentNullException(nameof(requestKeywords));

            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient.Dispose();
            }
        }
    }
}
