﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

/// <summary>
/// Represents an <see cref="IEqualityComparer{T}" /> that uses <see cref="Check.IsEquivalentTypeTo" />
/// to compare types. This check works like the normal type equality comparison, but when two
/// generic types are compared, they are regarded as equal when one of them is a constructed generic type
/// and the other one is the corresponding generic type definition.
/// </summary>
public sealed class EquivalentTypeComparer : IEqualityComparer<Type>
{
    /// <summary>
    /// Gets a singleton instance of the equality comparer.
    /// </summary>
    public static readonly EquivalentTypeComparer Instance = new ();

    /// <summary>
    /// Checks if the two types are equivalent (using <see cref="Check.IsEquivalentTypeTo" />).
    /// This check works like the normal type equality comparison, but when two
    /// generic types are compared, they are regarded as equal when one of them is a constructed generic type
    /// and the other one is the corresponding generic type definition.
    /// </summary>
    /// <param name="x">The first type.</param>
    /// <param name="y">The second type.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Type? x, Type? y) => x.IsEquivalentTypeTo(y);

    /// <summary>
    /// Returns the hash code of the given type. When the specified type is a constructed generic type,
    /// the hash code of the generic type definition is returned instead.
    /// </summary>
    /// <param name="type">The type whose hash code is requested.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(Type type) =>
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        type is null                  ? 0 :
        type.IsConstructedGenericType ? type.GetGenericTypeDefinition().GetHashCode() :
                                        type.GetHashCode();
}
