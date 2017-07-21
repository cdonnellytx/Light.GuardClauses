using System;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeValueTypeTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeValueType must throw a TypeException when the specified type is a reference type.")]
        public void IsReferenceType()
        {
            TestIsReferenceType(() => typeof(object).MustBeValueType(), typeof(object));
            TestIsReferenceType(() => typeof(IDisposable).GetTypeInfo().MustBeValueType(), typeof(IDisposable));
        }

        private static void TestIsReferenceType(Action act, Type type)
        {
            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must be a value type, but it is a reference type");
        }

        [Fact(DisplayName = "MustBeValueType must not throw an exception when the specified type is a value type.")]
        public void IsValueType()
        {
            TestIsValueType(() => typeof(double).MustBeValueType());
            TestIsValueType(() => typeof(ConsoleKey).GetTypeInfo().MustBeValueType());
        }

        private static void TestIsValueType(Action act)
        {
            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustBeValueType must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustBeValueType()).ShouldThrow<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustBeValueType()).ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(object).MustBeValueType(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(Action).MustBeValueType(message: message));

            testData.AddExceptionTest(exception => typeof(string).GetTypeInfo().MustBeValueType(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(IDisposable).GetTypeInfo().MustBeValueType(message: message));
        }
    }
}