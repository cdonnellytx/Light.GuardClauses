﻿using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeTrueTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeTrue must throw an ArgumentException when the specified boolean is true.")]
        public void BooleanFalse()
        {
            const bool itsTrue = true;

            Action act = () => itsTrue.MustNotBeTrue(nameof(itsTrue));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(itsTrue)} must not be true, but you specified true.");
        }

        [Fact(DisplayName = "MustNotBeTrue must not throw an exception when the specified boolean is false.")]
        public void BooleanTrue()
        {
            Action act = () => false.MustNotBeTrue();

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => true.MustNotBeTrue(exception: exception))
                    .AddMessageTest<ArgumentException>(message => true.MustNotBeTrue(message: message));
        }
    }
}