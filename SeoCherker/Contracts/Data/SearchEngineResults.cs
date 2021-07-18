using System;
using System.Collections.Generic;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts.Data
{
    public class SearchEngineResults
    {
        public SearchEngineResults(string searchEngine, IEnumerable<string> websiteNames, bool hasError = false)
        {
            SearchEngine = searchEngine ?? throw new ArgumentNullException(nameof(searchEngine));
            WebsiteNames = websiteNames ?? throw new ArgumentNullException(nameof(websiteNames));
            HasError = hasError;
        }

        public string SearchEngine { get; }

        public IEnumerable<string> WebsiteNames { get; }

        public bool HasError { get; }
    }
}