// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public class IsNullOrEmptyTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _methodInfo
            = typeof(string).GetRuntimeMethod(nameof(string.IsNullOrEmpty), new[] { typeof(string) });

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual Expression Translate(
            MethodCallExpression methodCallExpression,
            IDiagnosticsLogger<DbLoggerCategory.Query> logger)
        {
            Check.NotNull(methodCallExpression, nameof(methodCallExpression));

            return Equals(methodCallExpression.Method, _methodInfo)
                ? Expression.MakeBinary(
                    ExpressionType.OrElse,
                    new IsNullExpression(methodCallExpression.Arguments[0]),
                    Expression.Equal(methodCallExpression.Arguments[0], Expression.Constant("", typeof(string))))
                : null;
        }
    }
}
