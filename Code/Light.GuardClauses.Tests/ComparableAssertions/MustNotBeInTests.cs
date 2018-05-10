﻿using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions
{
    public static class MustNotBeInTests
    {
        [Theory]
        [InlineData(1, 1, 5)]
        [InlineData(2, 1, 5)]
        [InlineData(4, 1, 5)]
        [InlineData('b', 'b', 'f')]
        [InlineData('c', 'b', 'f')]
        [InlineData('e', 'b', 'f')]
        public static void ParameterWithinInclusiveLowerAndExclusiveUpperBoundary(int value, int lowerBoundary, int upperBoundary)
        {
            Action act = () => value.MustNotBeIn(Range<int>.FromInclusive(lowerBoundary).ToExclusive(upperBoundary), nameof(value));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be between {lowerBoundary} (inclusive) and {upperBoundary} (exclusive), but it actually is {value}.");
        }

        [Theory]
        [InlineData(2, 1, 5)]
        [InlineData(4, 1, 5)]
        [InlineData(5, 1, 5)]
        [InlineData('c', 'b', 'f')]
        [InlineData('d', 'b', 'f')]
        [InlineData('f', 'b', 'f')]
        public static void ParameterWithinExclusiveLowerAndInclusiveUpperBoundary(short value, short lowerBoundary, short upperBoundary)
        {
            Action act = () => value.MustNotBeIn(Range<short>.FromExclusive(lowerBoundary).ToInclusive(upperBoundary), nameof(value));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be between {lowerBoundary} (exclusive) and {upperBoundary} (inclusive), but it actually is {value}.");
        }

        [Theory]
        [InlineData(9, 10, 20, true, true)]
        [InlineData(21, 10, 20, true, true)]
        [InlineData(20, 10, 20, true, false)]
        [InlineData(10, 10, 20, false, false)]
        [InlineData(181, 10, 20, false, false)]
        public static void ParameterOutOfRange(int value, int lowerBoundary, int upperBoundary, bool isLowerBoundaryInclusive, bool isUpperBoundaryInclusive)
        {
            var range = new Range<int>(lowerBoundary, upperBoundary, isLowerBoundaryInclusive, isUpperBoundaryInclusive);
            var result = value.MustNotBeIn(range, nameof(value));

            result.Should().Be(value);
        }

        [Fact(DisplayName = "MustNotBeIn must throw the custom exception with two parameters when parameter is within the specified range.")]
        public static void ThrowCustomExceptionWithTwoParameters() =>
            Test.CustomException(12, new Range<int>(10, 15), (x, range, exceptionFactory) => x.MustNotBeIn(range, exceptionFactory));


        [Fact]
        public static void NoCustomExceptionThrown() =>
            (-15400.8m).MustNotBeIn(Range<decimal>.FromInclusive(0m).ToExclusive(100m), (v, b) => null).Should().Be(-15400.8m);
    }
}