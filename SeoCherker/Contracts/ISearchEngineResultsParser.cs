using System;
using System.Collections.Generic;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts
{
    public interface ISearchEngineResultsParser
    {
        IEnumerable<string> ParseResults(string rawResponse);

        Uri? ParseNextPageUri(string rawResponse);
    }
}