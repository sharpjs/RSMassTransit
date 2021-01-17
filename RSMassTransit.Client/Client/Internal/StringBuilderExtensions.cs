/*
    Copyright 2021 Jeffrey Sharp

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
using System.Text;

namespace RSMassTransit.Client.Internal
{
    /// <summary>
    ///   Extensions for <see cref="StringBuilder"/>.
    /// </summary>
    internal static class StringBuilderExtensions
    {
        /// <summary>
        ///   Appends a delimited, separated list to the
        ///   <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="builder">
        ///   The instance to which the list should be appended.
        /// </param>
        /// <param name="items">
        ///   The items from which to form the list.
        /// </param>
        /// <param name="separator">
        ///   The string to separate items in the list.
        /// </param>
        /// <param name="delimiter">
        ///   The string to delimit each item in the list.
        /// </param>
        /// <returns>
        ///   The <paramref name="builder"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        public static StringBuilder AppendDelimitedList(
            this StringBuilder   builder,
            IEnumerable<string>? items,
            string?              separator = ", ",
            string?              delimiter = "'")
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (items is null)
                return builder;

            using var e = items.GetEnumerator();

            if (!e.MoveNext())
                return builder;

            for (;;)
            {
                builder.Append(delimiter);
                builder.Append(e.Current);
                builder.Append(delimiter);

                if (!e.MoveNext())
                    return builder;

                builder.Append(separator);
            }
        }
    }
}
