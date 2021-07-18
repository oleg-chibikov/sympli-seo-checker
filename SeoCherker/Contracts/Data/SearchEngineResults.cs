using System;
using System.Collections.Generic;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts.Data
{
    public class SearchEngineResults
    {
        public SearchEngineResults(string searchEngine, IEnumerable<string> websiteNames)
        {
            SearchEngine = searchEngine ?? throw new ArgumentNullException(nameof(searchEngine));
            WebsiteNames = websiteNames ?? throw new ArgumentNullException(nameof(websiteNames));
        }

        public string SearchEngine { get; }

        public IEnumerable<string> WebsiteNames { get; }
    }
}