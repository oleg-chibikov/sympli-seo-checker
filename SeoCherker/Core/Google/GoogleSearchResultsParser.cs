using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;

namespace OlegChibikov.SympliInterview.SeoChecker.Core.Google
{
    public sealed class GoogleSearchResultsParser : ISearchEngineResultsParser
    {
        /// <summary>
        /// Google displays search results in an 'a' tag which has only 2 direct children: h3 and div. The latter contains the address of the website, that is of interest.
        /// The page may contain the 'Places on the map' section which also has a similar pattern. However, it contains some additional tags like span within the 'a'. This is not considered as a search result and therefore skipped.
        /// </summary>
        readonly Regex _resultTagsRegex = new (@"<a((?!span).)*?><h3.*?<\/h3>.*?<div.*?>(.*?)<\/div>", RegexOptions.Compiled);

        // Version for the page with User-agent
        // readonly Regex _nextPageRegex = new (@"<a href=""((?:(?!<a).)*?)"" id=""pnnext""", RegexOptions.Compiled);
        readonly Regex _nextPageRegex = new (@"<footer>.*?href=""(.*?)"".*?Next", RegexOptions.Compiled);

        public IEnumerable<string> ParseResults(string rawResponse) => _resultTagsRegex.Matches(rawResponse).Select(x => x.Groups[2].Value);

        public Uri? ParseNextPageUri(string rawResponse)
        {
            var nextPageMatch = _nextPageRegex.Match(rawResponse);
            return nextPageMatch.Success ? new Uri(HttpUtility.HtmlDecode(nextPageMatch.Groups[1].Value), UriKind.Relative) : null;
        }
    }
}