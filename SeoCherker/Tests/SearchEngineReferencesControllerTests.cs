using System;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OlegChibikov.SympliInterview.SeoChecker.Api.Data;

namespace OlegChibikov.SympliInterview.SeoChecker.Tests
{
    public class SearchEngineReferencesControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        static IOptionsMonitor<AppSettings> CreateMockOptionsMonitor(TimeSpan? cacheDuration = null)
        {
            var optionsMonitorMock = new Mock<IOptionsMonitor<AppSettings>>();
            optionsMonitorMock.SetupGet(x => x.CurrentValue).Returns(new AppSettings { CacheDuration = cacheDuration ?? TimeSpan.FromHours(1) });
            return optionsMonitorMock.Object;
        }
    }
}