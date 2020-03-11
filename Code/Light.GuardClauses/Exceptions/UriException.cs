﻿using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that an URI is invalid.
    /// </summary>
    [Serializable]
    public class UriException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="UriException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public UriException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

        /// <inheritdoc />
        protected UriException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
