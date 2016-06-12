﻿using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeGreaterThanTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeGreaterThan must throw an ArgumentOutOfRangeException when the boundary value is greater or equal to the parameter value to be checked.")]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(-1, -1)]
        [InlineData(0, 0)]
        [InlineData(short.MinValue, short.MinValue)]
        [InlineData('F', 'c')]
        [InlineData("aaa", "bbb")]
        public void ParameterEqualOrLess<T>(T first, T second) where T : IComparable<T>
        {
            Action act = () => first.MustBeGreaterThan(second, nameof(first));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(first)} must be greater than {second}, but you specified {first}.");
        }

        [Theory(DisplayName = "MustBeGreaterThan must not throw an exception when the specified parameter is greater than the boundary value.")]
        [InlineData(133, 99)]
        [InlineData(int.MaxValue, int.MaxValue - 1)]
        [InlineData(byte.MaxValue, byte.MinValue)]
        [InlineData('Z', 'X')]
        [InlineData("Hello", "ah yeah")]
        public void ParameterGreater<T>(T first, T second) where T : IComparable<T>
        {
            Action act = () => first.MustBeGreaterThan(second);

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 1.MustBeGreaterThan(2, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustBeGreaterThan(87, message: message)));
        }
    }
}