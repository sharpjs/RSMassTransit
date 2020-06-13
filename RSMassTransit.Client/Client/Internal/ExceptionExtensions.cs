/*
    Copyright (C) 2020 Jeffrey Sharp

    Permission to use, copy, modify, and distribute this software for any
    purpose with or without fee is hereby granted, provided that the above
    copyright notice and this permission notice appear in all copies.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
    WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
    MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
    ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
    WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
    ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
    OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

using System;
using System.Collections.Generic;

namespace RSMassTransit.Client.Internal
{
    internal static class ExceptionExtensions
    {
        /// <summary>
        ///   Returns the innermost nested exceptions.
        ///   For <c>AggregateException</c>, this method can return multiple items.
        /// </summary>
        /// <param name="exception">
        ///   An exception which might have nested inner exceptions.
        /// </param>
        /// <returns>
        ///   The exception(s) obtained by following the <c>InnerException</c>
        ///   or <c>InnerExceptions</c> properties exhaustively.
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
