using System.Threading;
using System.Threading.Tasks;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts
{
    public interface ISearchEngineResultsParser
    {
        SearchEngine SearchEngine { get; }

        Task<SearchEngineReferences> GetReferencesAsync(string requestKeywords, string reference, CancellationToken cancellationToken = default);
    }
}