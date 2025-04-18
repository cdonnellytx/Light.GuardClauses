using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the collection does not contain the specified item, or otherwise throws an <see cref="ExistingItemException" />.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="item">The item that must not be part of the collection.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ExistingItemException">Thrown when <paramref name="parameter" /> contains <paramref name="item" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TCollection MustNotContain<TCollection, TItem>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        TItem item,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where TCollection : class, IEnumerable<TItem>
    {
        if (parameter is ICollection<TItem> collection)
        {
            if (collection.Contains(item))
            {
                Throw.ExistingItem(parameter, item, parameterName, message);
            }

            return parameter;
        }

        if (parameter.MustNotBeNull(parameterName, message).Contains(item))
        {
            Throw.ExistingItem(parameter, item, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the collection does not contain the specified item, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="item">The item that must not be part of the collection.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="item" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> contains <paramref name="item" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TCollection MustNotContain<TCollection, TItem>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        TItem item,
        Func<TCollection?, TItem, Exception> exceptionFactory
    ) where TCollection : class, IEnumerable<TItem>
    {
        if (parameter is ICollection<TItem> collection)
        {
            if (collection.Contains(item))
            {
                Throw.CustomException(exceptionFactory, parameter, item);
            }

            return parameter;
        }

        if (parameter is null || parameter.Contains(item))
        {
            Throw.CustomException(exceptionFactory, parameter, item);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string does not contain the specified value, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The string that must not be part of <paramref name="parameter" />.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SubstringException">Thrown when <paramref name="parameter" /> contains <paramref name="value" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustNotContain(
        [NotNull] [ValidatedNotNull] this string? parameter,
        string value,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.MustNotBeNull(parameterName, message).Contains(value.MustNotBeNull(nameof(value), message)))
        {
            Throw.StringContains(parameter, value, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string does not contain the specified value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The string that must not be part of <paramref name="parameter" />.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception (optional). <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> contains <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustNotContain(
        [NotNull] [ValidatedNotNull] this string? parameter,
        string value,
        Func<string?, string, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || value is null || parameter.Contains(value))
        {
            Throw.CustomException(exceptionFactory, parameter, value!);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string does not contain the specified value, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The string that must not be part of <paramref name="parameter" />.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SubstringException">Thrown when <paramref name="parameter" /> contains <paramref name="value" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustNotContain(
        [NotNull] [ValidatedNotNull] this string? parameter,
        string value,
        StringComparison comparisonType,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.MustNotBeNull(parameterName, message).IndexOf(
                value.MustNotBeNull(nameof(value), message),
                comparisonType
            ) >=
            0)
        {
            Throw.StringContains(parameter, value, comparisonType, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string does not contain the specified value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The string that must not be part of <paramref name="parameter" />.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception (optional). <paramref name="parameter" />, <paramref name="value" />, and <paramref name="comparisonType" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> contains <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustNotContain(
        [NotNull] [ValidatedNotNull] this string? parameter,
        string value,
        StringComparison comparisonType,
        Func<string?, string, StringComparison, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || value is null || parameter.IndexOf(value, comparisonType) >= 0)
        {
            Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
        }

        return parameter;
    }
}
