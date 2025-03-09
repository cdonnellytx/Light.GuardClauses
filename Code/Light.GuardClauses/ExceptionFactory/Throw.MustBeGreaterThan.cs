using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must be greater
    /// than the given boundary value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeGreaterThan<T>(
        T parameter,
        T boundary,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(
            parameterName,
            message ??
            $"{parameterName ?? "The value"} must be greater than {boundary}, but it actually is {parameter}."
        );
}
