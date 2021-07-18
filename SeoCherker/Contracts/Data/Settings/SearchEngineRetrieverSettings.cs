using System;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts.Data.Settings
{
    public sealed class SearchEngineRetrieverSettings
    {
        public int RequiredResults { get; set; } = 100;

        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromHours(1);
    }
}
