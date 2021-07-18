using System;
using System.Collections.Generic;
using System.Linq;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;

namespace OlegChibikov.SympliInterview.SeoChecker.Core
{
    public sealed class ReferenceMatcher : IReferenceMatcher
    {
        public IEnumerable<int> MatchReferenceToResults(string reference, IEnumerable<string> results)
        {
            _ = results ?? throw new ArgumentNullException(nameof(results));
            _ = reference ?? throw new ArgumentNullException(nameof(reference));

            var i = 0;
            return results.Select(x => (Index: ++i, Text: x)).Where(x => x.Text.Contains(reference, StringComparison.InvariantCulture)).Select(x => x.Index);
        }
    }
}
