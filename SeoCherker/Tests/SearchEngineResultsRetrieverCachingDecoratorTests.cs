using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data.Settings;
using OlegChibikov.SympliInterview.SeoChecker.Core;

namespace OlegChibikov.SympliInterview.SeoChecker.Tests
{
    /// <remarks>
    /// Expiration should not be tested here as it is a part of an underlying cache.
    /// </remarks>
    public class SearchEngineResultsRetrieverCachingDecoratorTests
    {
        const string RequestKeywords = "request";
        static readonly string SearchEngineName = SearchEngine.Google.GetName();
        readonly SearchEngineResults _cachedResults = new (SearchEngineName, Array.Empty<string>());
        readonly SearchEngineResults _resultsFromRetriever = new (SearchEngineName, new[] { "1" });

        [Test]
        public async Task ReturnsCachedValue()
        {
            // Arrange
            var optionsMonitorMock = new Mock<IOptionsMonitor<CachingSettings>>();
            optionsMonitorMock.Setup(x => x.CurrentValue).Returns(new CachingSettings { CacheDuration = TimeSpan.FromDays(1) });
            var cacheMock = new Mock<ICache>();
            cacheMock.Setup(x => x.Get<SearchEngineResults>(It.IsAny<string>())).Returns(_cachedResults);

            var retrieverMock = new Mock<ISearchEngineResultsRetriever>();
            retrieverMock.Setup(x => x.GetSearchResultsAsync(RequestKeywords, It.IsAny<CancellationToken>())).ReturnsAsync(_resultsFromRetriever);

            var sut = new SearchEngineResultsRetrieverCachingDecorator(optionsMonitorMock.Object, cacheMock.Object, retrieverMock.Object);

            // Act
            await sut.GetSearchResultsAsync(RequestKeywords);

            // Assert
            cacheMock.VerifyAll();
            cacheMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<SearchEngineResults>(), It.IsAny<TimeSpan>()), Times.Never);
            retrieverMock.Verify(x => x.GetSearchResultsAsync(RequestKeywords, It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ReturnsNewValueAndSavesItToCache_When_CachedValueIsMissing()
        {
            // Arrange
            var optionsMonitorMock = new Mock<IOptionsMonitor<CachingSettings>>();
            optionsMonitorMock.Setup(x => x.CurrentValue).Returns(new CachingSettings { CacheDuration = TimeSpan.FromDays(1) });
            var cacheMock = new Mock<ICache>();
            cacheMock.Setup(x => x.Get<SearchEngineResults>(It.IsAny<string>())).Returns((SearchEngineResults?)null);

            var retrieverMock = new Mock<ISearchEngineResultsRetriever>();
            retrieverMock.Setup(x => x.GetSearchResultsAsync(RequestKeywords, It.IsAny<CancellationToken>())).ReturnsAsync(_resultsFromRetriever);

            var sut = new SearchEngineResultsRetrieverCachingDecorator(optionsMonitorMock.Object, cacheMock.Object, retrieverMock.Object);

            // Act
            await sut.GetSearchResultsAsync(RequestKeywords);

            // Assert
            cacheMock.VerifyAll();
            cacheMock.Verify(x => x.Set(It.IsAny<string>(), _resultsFromRetriever, It.IsAny<TimeSpan>()), Times.Once);
            retrieverMock.Verify(x => x.GetSearchResultsAsync(RequestKeywords, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}