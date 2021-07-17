using System;
using System.Collections.Generic;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts.Data
{
    public class SearchEngineReferences
    {
        public SearchEngineReferences(SearchEngine searchEngine, IEnumerable<int> resultOrderNumbers)
        {
            SearchEngine = searchEngine;
            IndexesInSearchResults = resultOrderNumbers ?? throw new ArgumentNullException(nameof(resultOrderNumbers));
        }

        SearchEngine SearchEngine { get; }

        /// <summary>
        /// The collection of numbers, starting from 1, showing the order number of each search result which was returned for the requested keywords.
        /// </summary>
        IEnumerable<int> IndexesInSearchResults { get; }
    }
}
