using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using OlegChibikov.SympliInterview.SeoChecker.Api.Controllers;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data;

namespace OlegChibikov.SympliInterview.SeoChecker.Tests
{
    public class SearchEngineReferencesControllerTests
    {
        [Test]
        public async Task CombinesResultsFromDifferentRetrievers()
        {
            // Arrange
            const string reference = "find me";
            var retriever1Mock = new Mock<ISearchEngineResultsRetriever>();
            var retriever1Results = new[] { "site1", "site2" };
            var retriever1SearchEngine = SearchEngine.Google.GetName();
            var retriever1ExpectedIndexes = new[] { 1 };
            retriever1Mock.Setup(x => x.GetSearchResultsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SearchEngineResults(retriever1SearchEngine, retriever1Results));

            var retriever2Mock = new Mock<ISearchEngineResultsRetriever>();
            var retriever2Results = new[] { "site3", "site4", "site5" };
            var retriever2SearchEngine = SearchEngine.Bing.GetName();
            var retriever2ExpectedIndexes = new[] { 2, 3 };
            retriever2Mock.Setup(x => x.GetSearchResultsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SearchEngineResults(retriever2SearchEngine, retriever2Results));

            var referenceMatcherMock = new Mock<IReferenceMatcher>();
            referenceMatcherMock.Setup(x => x.MatchReferenceToResults(reference, retriever1Results)).Returns(retriever1ExpectedIndexes);
            referenceMatcherMock.Setup(x => x.MatchReferenceToResults(reference, retriever2Results)).Returns(retriever2ExpectedIndexes);
            var sut = new SearchEngineReferencesController(new[] { retriever1Mock.Object, retriever2Mock.Object }, referenceMatcherMock.Object);

            // Act
            var result = (await sut.GetAsync(reference: reference, cancellationToken: default).ConfigureAwait(false)).ToArray();

            // Assert
            result.Should().HaveCount(2);
            var first = result.First();
            var second = result.ElementAt(1);
            first.SearchEngine.Should().Be(retriever1SearchEngine);
            second.SearchEngine.Should().Be(retriever2SearchEngine);
            first.IndexesInSearchResults.Should().Equal(retriever1ExpectedIndexes);
            second.IndexesInSearchResults.Should().Equal(retriever2ExpectedIndexes);
            retriever1Mock.VerifyAll();
            retriever2Mock.VerifyAll();
            referenceMatcherMock.VerifyAll();
        }
    }
}