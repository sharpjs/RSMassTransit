using System;
using System.Collections.Generic;

namespace RSMassTransit.PowerShell
{
    internal static class ExceptionExtensions
    {
        /// <summary>
        ///   Returns the innermost nested exceptions.
        ///   For <c>AggregateException</c>, this method can return multiple items.
        /// </summary>
        /// <param name="exception">
        ///   An exception which might have nested <c>InnerException</c>s.
        /// </param>
        /// <returns>
        ///   The exception obtained by following the <c>InnerException</c>
        ///   or <c>InnerExceptions</c> properties as many times as possible.
        /// </returns>
        public static IEnumerable<Exception> GetBaseExceptions(this Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            for (;;)
            {
                // When multiple inner exceptions, recurse for each.
                var aggregate = exception as AggregateException;
                if (aggregate?.InnerExceptions?.Count > 1)
                {
                    foreach (var inner_ in aggregate.InnerExceptions)
                    foreach (var base_  in inner_.GetBaseExceptions())
                        yield return base_;
                    yield break;
                }

                // When no inner exception, return immediately.
                var inner = exception.InnerException;
                if (inner == null)
                {
                    yield return exception;
                    yield break;
                }

                // One inner exception.  Drill down and continue.
                exception = inner;
            }
        }
    }
}
