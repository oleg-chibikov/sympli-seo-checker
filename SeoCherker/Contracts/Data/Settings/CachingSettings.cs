using System;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts.Data.Settings
{
    public sealed class CachingSettings
    {
        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromHours(1);
    }
}
