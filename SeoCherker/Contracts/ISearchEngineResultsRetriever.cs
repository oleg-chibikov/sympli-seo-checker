﻿using System.Threading;
using System.Threading.Tasks;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts
{
    public interface ISearchEngineResultsRetriever
    {
        SearchEngine SearchEngine { get; }

        Task<SearchEngineResults> GetSearchResultsAsync(string requestKeywords, CancellationToken cancellationToken = default);
    }
}