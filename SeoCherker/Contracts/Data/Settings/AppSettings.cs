using System;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts.Data.Settings
{
    public sealed class AppSettings
    {
        public string? WebAppHost { get; set; }

        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromHours(1);
    }
}
