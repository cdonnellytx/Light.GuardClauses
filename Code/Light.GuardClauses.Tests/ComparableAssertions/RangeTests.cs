﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class RangeTests
{
    [Theory]
    [InlineData(0, 5, 0, true, true)]
    [InlineData(0, 5, 5, false, true)]
    [InlineData(0, 5, 4, false, false)]
    [InlineData(0, 5, 1, false, false)]
    [InlineData(-4, 4, 0, false, false)]
    [InlineData(42, 80, 42, true, false)]
    public static void ValueInRange(long from, long to, long value, bool isFromInclusive, bool isToInclusive)
    {
        var testTarget = new Range<long>(from, to, isFromInclusive, isToInclusive);

        var result = testTarget.IsValueWithinRange(value);

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 5, -1, true, true)]
    [InlineData(0, 5, 6, false, true)]
    [InlineData(0, 5, 5, false, false)]
    [InlineData(0, 5, 0, false, false)]
    [InlineData(-4, 4, -80, false, false)]
    [InlineData(42, 80, 42, false, false)]
    public static void ValueOutOfRange(int from, int to, int value, bool isFromInclusive, bool isToInclusive)
    {
        var testTarget = new Range<int>(from, to, isFromInclusive, isToInclusive);

        var result = testTarget.IsValueWithinRange(value);

        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(42, -1)]
    [InlineData(-87, -88)]
    public static void ConstructorException(int from, int to)
    {
        Action act = () => Range<int>.FromInclusive(from).ToExclusive(to);

        act.Should().Throw<ArgumentOutOfRangeException>().And
           .Message.Should().Contain($"{nameof(to)} must not be less than {from}, but it actually is {to}.");
    }

    [Theory]
    [MemberData(nameof(Collections))]
    public static void RangeForCollections(IEnumerable enumerable)
    {
        // ReSharper disable PossibleMultipleEnumeration
        var range = Range.For(enumerable);

        var expectedRange = new Range<int>(0, enumerable.Count(), true, false);
        range.Should().Be(expectedRange);
        // ReSharper restore PossibleMultipleEnumeration
    }

    public static readonly TheoryData<IEnumerable> Collections =
        new ()
        {
            new List<int> { 1, 2 ,3 ,4},
            "This is a long string",
            new[] { 'a', 'b', 'c', 'd' },
            new ObservableCollection<long> { 1, -1 },
            new ArrayList()
        };

    [Theory]
    [MemberData(nameof(Memories))]
    public static void RangeForMemory(Memory<int> memory)
    {
        var range = Range.For(memory);

        var expectedRange = new Range<int>(0, memory.Length, true, false);
        range.Should().Be(expectedRange);
    }

    [Theory]
    [MemberData(nameof(Memories))]
    public static void RangeForReadOnlyMemory(Memory<int> memory)
    {
        ReadOnlyMemory<int> readOnlyMemory = memory;
        var range = Range.For(readOnlyMemory);
        
        var expectedRange = new Range<int>(0, memory.Length, true, false);
        range.Should().Be(expectedRange);
    }

    [Theory]
    [MemberData(nameof(Memories))]
    public static void RangeForSpan(Memory<int> memory)
    {
        var range = Range.For(memory.Span);
        
        var expectedRange = new Range<int>(0, memory.Length, true, false);
        range.Should().Be(expectedRange);
    }

    [Theory]
    [MemberData(nameof(Memories))]
    public static void RangeForReadOnlySpan(Memory<int> memory)
    {
        ReadOnlySpan<int> readOnlySpan = memory.Span;
        var range = Range.For(readOnlySpan);
        
        var expectedRange = new Range<int>(0, memory.Length, true, false);
        range.Should().Be(expectedRange);
    }

    public static readonly TheoryData<Memory<int>> Memories =
        new ()
        {
            new [] { 1, 2, 3, 4 },
            Enumerable.Range(1, 500).ToArray(),
            Array.Empty<int>()
        };
}