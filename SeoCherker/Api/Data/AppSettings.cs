using System;

namespace OlegChibikov.SympliInterview.SeoChecker.Api.Data
{
    public class AppSettings
    {
        public string? WebAppHost { get; set; }

        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromHours(1);
    }
}
