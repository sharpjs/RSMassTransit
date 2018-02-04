// Copyright (C) 2018 (to be determined)

using System;
using System.Collections.Generic;

namespace RSMassTransit.PowerShell
{
    internal static class EnumerableExtensions
    {
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
