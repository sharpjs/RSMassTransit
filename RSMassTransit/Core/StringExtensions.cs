// Copyright (C) 2018 (to be determined)

namespace RSMassTransit
{
    internal static class StringExtensions
    {
        public static string NullIfEmpty(this string s)
            => s == "" ? null : s;
    }
}
