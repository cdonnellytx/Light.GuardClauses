using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the value is not one of the specified items, or otherwise throws a <see cref="ValueIsOneOfException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="items">The items that must not contain the value.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ValueIsOneOfException">Thrown when <paramref name="parameter" /> is equal to one of the specified <paramref name="items" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="items" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("items:null => halt")]
    // ReSharper disable once RedundantNullableFlowAttribute - the attribute has an effect, see Issue72NotNullAttribute tests
    public static TItem MustNotBeOneOf<TItem>(
        this TItem parameter,
        [NotNull] [ValidatedNotNull] IEnumerable<TItem> items,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        // ReSharper disable PossibleMultipleEnumeration
        if (parameter.IsOneOf(items.MustNotBeNull(nameof(items), message)))
        {
            Throw.ValueIsOneOf(parameter, items, parameterName, message);
        }

        return parameter;
        // ReSharper restore PossibleMultipleEnumeration
    }

    /// <summary>
    /// Ensures that the value is not one of the specified items, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="items">The items that must not contain the value.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="items" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is equal to one of the specified <paramref name="items" />, or when <paramref name="items" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("items:null => halt")]
    public static TItem MustNotBeOneOf<TItem, TCollection>(
        this TItem parameter,
        [NotNull] [ValidatedNotNull] TCollection items,
        Func<TItem, TCollection, Exception> exceptionFactory
    ) where TCollection : class, IEnumerable<TItem>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (items is null || parameter.IsOneOf(items))
        {
            Throw.CustomException(exceptionFactory, parameter, items!);
        }

        return parameter;
    }
}
