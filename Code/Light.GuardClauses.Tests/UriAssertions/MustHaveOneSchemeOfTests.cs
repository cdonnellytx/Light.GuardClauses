﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.UriAssertions
{
    public static class MustHaveOneSchemeOfTests
    {
        [Theory]
        [InlineData("http://foo.com/", new[] { "ftp", "ftps" })]
        [InlineData("fttps://bar.com/", new[] { "http", "https" })]
        public static void SchemeNotPresent(string uri, string[] schemes)
        {
            Action act = () => new Uri(uri).MustHaveOneSchemeOf(schemes, nameof(uri));

            var exceptionAssertion = act.Should().Throw<InvalidUriSchemeException>().Which;
            exceptionAssertion.Message.Should().Contain(new StringBuilder().AppendLine($"{nameof(uri)} must use one of the following schemes")
                                                                           .AppendItemsWithNewLine(schemes)
                                                                           .AppendLine($"but it actually is \"{uri}\".")
                                                                           .ToString());
            exceptionAssertion.ParamName.Should().BeSameAs(nameof(uri));
        }

        [Theory]
        [InlineData("http://www.feO2x.com", new[] { "http", "https" })]
        [InlineData("https://www.microsoft.com", new[] { "http", "https" })]
        public static void SchemePresent(string uri, string[] schemes)
        {
            var instance = new Uri(uri);

            instance.MustHaveOneSchemeOf(schemes).Should().BeSameAs(instance);
        }

        [Fact]
        public static void UriNull()
        {
            Action act = () => ((Uri) null).MustHaveOneSchemeOf(Array.Empty<string>());

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void UriRelative()
        {
            Action act = () => new Uri("/api/foo", UriKind.Relative).MustHaveOneSchemeOf(Array.Empty<string>());

            act.Should().Throw<RelativeUriException>();
        }

        [Theory]
        [MemberData(nameof(CustomExceptionData))]
        public static void CustomException(Uri url, List<string> urlSchemes) =>
            Test.CustomException(url,
                                 urlSchemes,
                                 (uri, schemes, exceptionFactory) => uri.MustHaveOneSchemeOf(schemes, exceptionFactory));

        public static readonly TheoryData<Uri, List<string>> CustomExceptionData =
            new ()
            {
                { new Uri("https://www.microsoft.com"), new List<string> { "http", "ftp" } },
                { null, new List<string> { "http", "https" } },
                { new Uri("https://github.com"), null }
            };

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<InvalidUriSchemeException>(message => new Uri("https://go.com").MustHaveOneSchemeOf(new[] { "http" }, message: message));

        [Fact]
        public static void CustomMessageUrlRelative() =>
            Test.CustomMessage<RelativeUriException>(message => new Uri("/api", UriKind.Relative).MustHaveOneSchemeOf(new[] { "https" }, message: message));

        [Fact]
        public static void CustomMessageUrlNull() =>
            Test.CustomMessage<ArgumentNullException>(message => ((Uri) null).MustHaveOneSchemeOf(new[] { "http" }, message: message));

        [Fact]
        public static void CustomMessageSchemesNull() =>
            Test.CustomMessage<ArgumentNullException>(message => new Uri("https://www.dict.cc").MustHaveOneSchemeOf(null!, message: message));

        [Fact]
        public static void CallerArgumentExpression()
        {
            var myUrl = new Uri("https://www.synnotech.de", UriKind.Absolute);

            var act = () => myUrl.MustHaveOneSchemeOf(new[] { "ftp", "ftps" });

            act.Should().Throw<InvalidUriSchemeException>()
               .And.ParamName.Should().Be(nameof(myUrl));
        }
    }
}