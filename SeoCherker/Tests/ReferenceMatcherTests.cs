using System;
using FluentAssertions;
using NUnit.Framework;
using OlegChibikov.SympliInterview.SeoChecker.Core;

namespace OlegChibikov.SympliInterview.SeoChecker.Tests
{
    public class ReferenceMatcherTests
    {
        [Test]
        public void FindsMultipleMatchesAndReturnsOneBasedIndexes()
        {
            // Arrange
            var results = new[]
            {
                "www.sympli.com.au",
                "pexa.com",
                "some other site",
                "sympli.com.au"
            };
            const string reference = "sympli.com.au";
            var sut = new ReferenceMatcher();

            // Act
            var result = sut.MatchReferenceToResults(reference, results);

            // Assert
            result.Should().Equal(new[] { 1, 4 });
        }

        [Test]
        public void ReturnsEmptyEnumerable_When_ThereAreNoMatches()
        {
            // Arrange
            var results = new[]
            {
                "www.sympli.com.au",
                "pexa.com",
            };
            const string reference = "Something else";
            var sut = new ReferenceMatcher();

            // Act
            var result = sut.MatchReferenceToResults(reference, results);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void ReturnsEmptyEnumerable_When_EmptyCollectionIsPassed()
        {
            // Arrange
            var results = Array.Empty<string>();
            const string reference = "I am not here";
            var sut = new ReferenceMatcher();

            // Act
            var result = sut.MatchReferenceToResults(reference, results);

            // Assert
            result.Should().BeEmpty();
        }
    }
}