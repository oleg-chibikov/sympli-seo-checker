using System;
using Microsoft.Extensions.Options;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data.Settings;

namespace OlegChibikov.SympliInterview.SeoChecker.Core.Google
{
    public sealed class GoogleQueryProvider : IQueryProvider
    {
        readonly IOptionsMonitor<SearchEngineRetrieverSettings> _optionsMonitor;

        public GoogleQueryProvider(IOptionsMonitor<SearchEngineRetrieverSettings> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
        }

        public string GetRelativeUriPart(string requestKeywords)
        {
            var pageSize = Math.Min(_optionsMonitor.CurrentValue.RequiredResults, 100);
            return $"search?q={requestKeywords}&num={pageSize}";
        }
    }
}