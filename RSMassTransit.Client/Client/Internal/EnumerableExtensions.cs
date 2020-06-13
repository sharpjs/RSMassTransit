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
    internal static class EnumerableExtensions
    {
        /// <summary>
        ///   Attempts to get the only element of a sequence, failing if the
        ///   sequence contains no items or multiple items.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence from which to get the single element.</param>
        /// <param name="item">
        ///   When this method returns, contains the single element of
        ///   <paramref name="source"/>, or the default value of type
        ///   <typeparamref name="T"/> if the sequence is empty or contains
        ///   multiple elements.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the sequence contains a single element;
        ///   <c>false</c> otherwise.
        /// </returns>
        public static bool TrySingle<T>(this IEnumerable<T> source, out T item)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                item = default(T);
                return false;
            }

            var first = enumerator.Current;

            if (enumerator.MoveNext())
            {
                item = default(T);
                return false;
            }

            item = first;
            return true;
        }
    }
}
