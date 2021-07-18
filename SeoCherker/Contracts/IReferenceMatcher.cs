using System.Collections.Generic;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts
{
    public interface IReferenceMatcher
    {
        IEnumerable<int> MatchReferenceToResults(string reference, IEnumerable<string> results);
    }
}