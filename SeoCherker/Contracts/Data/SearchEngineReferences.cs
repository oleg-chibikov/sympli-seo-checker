using System;
using System.Collections.Generic;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts.Data
{
    public class SearchEngineReferences
    {
        public SearchEngineReferences(string searchEngine, IEnumerable<int> resultOrderNumbers, bool hasError = false)
        {
            SearchEngine = searchEngine ?? throw new ArgumentNullException(nameof(searchEngine));
            IndexesInSearchResults = resultOrderNumbers ?? throw new ArgumentNullException(nameof(resultOrderNumbers));
            HasError = hasError;
        }

        public string SearchEngine { get; }

        /// <summary>
        /// The collection of numbers, starting from 1, showing the order number of each search result which was returned for the requested keywords.
        /// </summary>
        public IEnumerable<int> IndexesInSearchResults { get; }

        public bool HasError { get; }
    }
}
