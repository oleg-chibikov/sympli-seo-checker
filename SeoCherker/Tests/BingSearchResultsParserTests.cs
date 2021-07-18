using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using OlegChibikov.SympliInterview.SeoChecker.Core.Bing;

namespace OlegChibikov.SympliInterview.SeoChecker.Tests
{
    public class BingSearchResultsParserTests
    {
        [Test]
        public void ParsesResults()
        {
            // Arrange
            var realResponse = ResourceHelper.ReadEmbeddedResource("BingResponse.html");
            var sut = new BingSearchResultsParser();

            // Act
            var result = sut.ParseResults(realResponse).ToArray();

            // Assert
            result.Should().HaveCount(49);
            result[1].Should().Be("https://www.sympli.com.au");
        }

        [Test]
        public void ParsesNextPageUri()
        {
            // Arrange
            var realResponse = ResourceHelper.ReadEmbeddedResource("BingResponse.html");
            var sut = new BingSearchResultsParser();

            // Act
            var result = sut.ParseNextPageUri(realResponse);

            // Assert
            result.Should().NotBeNull();
            result!.IsAbsoluteUri.Should().BeFalse();
            result.ToString().Should().Be("/search?q=e-settlements&count=50&toWww=1&redig=E57ACBB826FD4CDA9004DA8742CFD1E2&first=49&FORM=PORE");
        }

        [Test]
        public void ParsesResults_When_ThereIsNoData()
        {
            // Arrange
            var realResponse = ResourceHelper.ReadEmbeddedResource("BingResponseNoData.html");
            var sut = new BingSearchResultsParser();

            // Act
            var result = sut.ParseResults(realResponse).ToArray();

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void ParsesNextPageUri_When_ThereIsNoData()
        {
            // Arrange
            var realResponse = ResourceHelper.ReadEmbeddedResource("BingResponseNoData.html");
            var sut = new BingSearchResultsParser();

            // Act
            var result = sut.ParseNextPageUri(realResponse);

            // Assert
            result.Should().BeNull();
        }
    }
}
