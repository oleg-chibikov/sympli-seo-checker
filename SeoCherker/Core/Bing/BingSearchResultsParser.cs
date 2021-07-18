using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;

namespace OlegChibikov.SympliInterview.SeoChecker.Core.Bing
{
    public sealed class BingSearchResultsParser : ISearchEngineResultsParser
    {
        /// <summary>
        /// Bing returns the results wrapped in a cite tag. This tag is a child of a div with a class b_attribution (and no other classes).
        /// The results starting from Bing.com are not considered the search results (bing.com/videos, bing.com/images).
        /// Also it might return a link to Wikipedia, for which the div doesn't have any class. According to the paging rules of Bing, this is also considered a search result.
        /// </summary>
        readonly Regex _resultTagsRegex = new (@"<div( class=""b_attribution"".*?)?><cite>(.*?)<\/cite>", RegexOptions.Compiled);

        /// <summary>
        /// Finds the elements of the pager as a single string.
        /// </summary>
        readonly Regex _nextPageRegex = new (@"title=""Next page"" href=""(.*?)""", RegexOptions.Compiled);

        public IEnumerable<string> ParseResults(string rawResponse) =>
            _resultTagsRegex.Matches(rawResponse).Select(x => x.Groups[2].Value).Where(x => !x.Contains("bing.com/", StringComparison.OrdinalIgnoreCase));

        public Uri? ParseNextPageUri(string rawResponse)
        {
            var nextPageMatch = _nextPageRegex.Match(rawResponse);
            return !nextPageMatch.Success ? null : new Uri(HttpUtility.HtmlDecode(nextPageMatch.Groups[1].Value), UriKind.Relative);
        }
    }
}