using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
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
    public class SearchEngineResultsRetrieverTests
    {
        const SearchEngine Engine = SearchEngine.Google;
        readonly ILogger<SearchEngineResultsRetriever> _mockLogger = Mock.Of<ILogger<SearchEngineResultsRetriever>>();

        static string[] SampleResults
        {
            get { return new[] { "www.sympli.com.au", "pexa.com", "something else" }; }
        }

        [Test]
        public async Task ReturnsResultsWithoutRequestingAdditionalPage_When_NumberExceedsOrEqualToRequiredResults([Values(true, false)] bool resultsCountIsEqualToRequestedValue)
        {
            // Arrange
            var httpInvocationCount = 0;
            var httpClientFactory = CreateMockHttpClientFactory(() => httpInvocationCount++);
            var results = SampleResults;

            var requiredResults = results.Length + (resultsCountIsEqualToRequestedValue ? 0 : -1);
            var optionsMonitor = CreateMockOptionsMonitor(requiredResults);
            var (searchEngineResultsParserResolverMock, searchEngineResultsParserMock) = CreateMockSearchEngineResultsParser(results);
            var (queryProviderResolverMock, queryProviderMock) = CreateMockQueryProvider();

            var sut = new SearchEngineResultsRetriever(httpClientFactory, optionsMonitor, searchEngineResultsParserResolverMock.Object, queryProviderResolverMock.Object, Engine, _mockLogger);

            // Act
            var searchResults = await sut.GetSearchResultsAsync("doesn't matter");

            // Assert
            queryProviderMock.VerifyAll();
            searchEngineResultsParserMock.VerifyAll();
            searchEngineResultsParserMock.Verify(x => x.ParseNextPageUri(It.IsAny<string>()), Times.Never);
            httpInvocationCount.Should().Be(1);
            searchResults.WebsiteNames.Should().HaveCount(requiredResults);
        }

        [Test]
        public async Task RequestsAdditionalPage_When_NumberIsLessThanRequiredResults_And_NextPageExists()
        {
            // Arrange
            var httpInvocationCount = 0;
            var httpClientFactory = CreateMockHttpClientFactory(() => httpInvocationCount++);
            var results = SampleResults;

            var requiredResults = results.Length + 1;
            var optionsMonitor = CreateMockOptionsMonitor(requiredResults);
            var (searchEngineResultsParserResolverMock, searchEngineResultsParserMock) = CreateMockSearchEngineResultsParser(results, new Uri("/next/page", UriKind.Relative));
            var (queryProviderResolverMock, queryProviderMock) = CreateMockQueryProvider();

            var sut = new SearchEngineResultsRetriever(httpClientFactory, optionsMonitor, searchEngineResultsParserResolverMock.Object, queryProviderResolverMock.Object, Engine, _mockLogger);

            // Act
            var searchResults = await sut.GetSearchResultsAsync("doesn't matter");

            // Assert
            queryProviderMock.VerifyAll();
            searchEngineResultsParserMock.VerifyAll();
            searchEngineResultsParserMock.Verify(x => x.ParseNextPageUri(It.IsAny<string>()), Times.Once);
            httpInvocationCount.Should().Be(2);
            searchResults.WebsiteNames.Should().HaveCount(requiredResults);
        }

        [Test]
        public async Task ReturnsResultsOfTheFirstPageOnly_When_NumberIsLessThanRequiredResults_And_NextPageDoesNotExist()
        {
            // Arrange
            var httpInvocationCount = 0;
            var httpClientFactory = CreateMockHttpClientFactory(() => httpInvocationCount++);
            var results = SampleResults;

            var requiredResults = results.Length + 1;
            var optionsMonitor = CreateMockOptionsMonitor(requiredResults);
            var (searchEngineResultsParserResolverMock, searchEngineResultsParserMock) = CreateMockSearchEngineResultsParser(results);
            var (queryProviderResolverMock, queryProviderMock) = CreateMockQueryProvider();

            var sut = new SearchEngineResultsRetriever(httpClientFactory, optionsMonitor, searchEngineResultsParserResolverMock.Object, queryProviderResolverMock.Object, Engine, _mockLogger);

            // Act
            var searchResults = await sut.GetSearchResultsAsync("doesn't matter");

            // Assert
            queryProviderMock.VerifyAll();
            searchEngineResultsParserMock.VerifyAll();
            searchEngineResultsParserMock.Verify(x => x.ParseNextPageUri(It.IsAny<string>()), Times.Once);
            httpInvocationCount.Should().Be(1);
            searchResults.WebsiteNames.Should().HaveCount(results.Length);
        }

        [Test]
        public async Task SetsHasErrorToTrue_When_HttpClientReturnsError()
        {
            // Arrange
            var httpInvocationCount = 0;
            var httpClientFactory = CreateMockHttpClientFactory(() => httpInvocationCount++, HttpStatusCode.InternalServerError);
            var results = SampleResults;

            var requiredResults = results.Length;
            var optionsMonitor = CreateMockOptionsMonitor(requiredResults);
            var (searchEngineResultsParserResolverMock, searchEngineResultsParserMock) = CreateMockSearchEngineResultsParser(results);
            var (queryProviderResolverMock, queryProviderMock) = CreateMockQueryProvider();

            var sut = new SearchEngineResultsRetriever(httpClientFactory, optionsMonitor, searchEngineResultsParserResolverMock.Object, queryProviderResolverMock.Object, Engine, _mockLogger);

            // Act
            var searchResults = await sut.GetSearchResultsAsync("doesn't matter");

            // Assert
            queryProviderMock.VerifyAll();
            searchEngineResultsParserMock.Verify(x => x.ParseResults(It.IsAny<string>()), Times.Never);
            searchEngineResultsParserMock.Verify(x => x.ParseNextPageUri(It.IsAny<string>()), Times.Never);
            httpInvocationCount.Should().Be(1);
            searchResults.WebsiteNames.Should().HaveCount(0);
            searchResults.HasError.Should().BeTrue();
        }

        static (Mock<Func<SearchEngine, IQueryProvider>>, Mock<IQueryProvider>) CreateMockQueryProvider()
        {
            var queryProviderResolverMock = new Mock<Func<SearchEngine, IQueryProvider>>();
            var queryProviderMock = new Mock<IQueryProvider>();
            queryProviderMock.Setup(x => x.GetRelativeUriPart(It.IsAny<string>())).Returns("/some/uri");
            queryProviderResolverMock.Setup(x => x.Invoke(Engine)).Returns(queryProviderMock.Object);
            return (queryProviderResolverMock, queryProviderMock);
        }

        static (Mock<Func<SearchEngine, ISearchEngineResultsParser>>, Mock<ISearchEngineResultsParser>) CreateMockSearchEngineResultsParser(IEnumerable<string> results, Uri? nextPageUri = null)
        {
            var searchEngineResultsParserResolverMock = new Mock<Func<SearchEngine, ISearchEngineResultsParser>>();
            var searchEngineResultsParserMock = new Mock<ISearchEngineResultsParser>();
            searchEngineResultsParserMock.Setup(x => x.ParseResults(It.IsAny<string>())).Returns(results);
            if (nextPageUri != null)
            {
                searchEngineResultsParserMock.Setup(x => x.ParseNextPageUri(It.IsAny<string>())).Returns(nextPageUri);
            }

            searchEngineResultsParserResolverMock.Setup(x => x.Invoke(Engine)).Returns(searchEngineResultsParserMock.Object);
            return (searchEngineResultsParserResolverMock, searchEngineResultsParserMock);
        }

        static IOptionsMonitor<SearchEngineRetrieverSettings> CreateMockOptionsMonitor(int requiredResults)
        {
            var optionsMonitorMock = new Mock<IOptionsMonitor<SearchEngineRetrieverSettings>>();
            optionsMonitorMock.Setup(x => x.CurrentValue).Returns(new SearchEngineRetrieverSettings { RequiredResults = requiredResults });
            return optionsMonitorMock.Object;
        }

        static IHttpClientFactory CreateMockHttpClientFactory(Action? httpInvocationCallback = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpMessageHandlerMock = new MessageHandlerStub((_, _) =>
            {
                httpInvocationCallback?.Invoke();
                return Task.FromResult(new HttpResponseMessage(statusCode));
            });

            var client = new HttpClient(httpMessageHandlerMock)
            {
                BaseAddress = new Uri("http://example.com")
            };

            httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            return httpClientFactoryMock.Object;
        }

        class MessageHandlerStub : DelegatingHandler
        {
            readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;

            public MessageHandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
            {
                _handlerFunc = handlerFunc;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return _handlerFunc(request, cancellationToken);
            }
        }
    }
}