using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using OlegChibikov.SympliInterview.SeoChecker.Core.Google;

namespace OlegChibikov.SympliInterview.SeoChecker.Tests
{
    public class GoogleSearchResultsParserTests
    {
        [Test]
        public void ParsesResults()
        {
            // Arrange
            var realResponse = ResourceHelper.ReadEmbeddedResource("GoogleResponse.html");
            var sut = new GoogleSearchResultsParser();

            // Act
            var result = sut.ParseResults(realResponse).ToArray();

            // Assert
            result.Should().HaveCount(100);
            result[1].Should().Be("www.sympli.com.au");
        }

        [Test]
        public void ParsesNextPageUri()
        {
            // Arrange
            var realResponse = ResourceHelper.ReadEmbeddedResource("GoogleResponse.html");
            var sut = new GoogleSearchResultsParser();

            // Act
            var result = sut.ParseNextPageUri(realResponse);

            // Assert
            result.Should().NotBeNull();
            result!.IsAbsoluteUri.Should().BeFalse();
            result.ToString().Should().Be("/search?q=e-settlements&num=100&ie=UTF-8&ei=UTT0YIT6O7fA0PEPqemXiAw&start=100&sa=N");
        }

        [Test]
        public void ParsesResults_When_ThereIsNoData()
        {
            // Arrange
            var realResponse = ResourceHelper.ReadEmbeddedResource("GoogleResponseNoData.html");
            var sut = new GoogleSearchResultsParser();

            // Act
            var result = sut.ParseResults(realResponse).ToArray();

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void ParsesNextPageUri_When_ThereIsNoData()
        {
            // Arrange
            var realResponse = ResourceHelper.ReadEmbeddedResource("GoogleResponseNoData.html");
            var sut = new GoogleSearchResultsParser();

            // Act
            var result = sut.ParseNextPageUri(realResponse);

            // Assert
            result.Should().BeNull();
        }
    }
}
