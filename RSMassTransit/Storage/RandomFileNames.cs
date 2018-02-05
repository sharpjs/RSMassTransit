/*
    Copyright (C) 2018 (to be determined)

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

using static System.FormattableString;

namespace RSMassTransit.Storage
{
    internal static class RandomFileNames
    {
        private static readonly Random
            Random = CreateRandom();

        internal static string Next(char separator = '.', string extension = "")
        {
            // Format: yyyy/MMdd/yyyyMMdd_HHmmss_xxxxxxxx.ext

            var d = DateTime.UtcNow;
            var n = GetRandomInt32();
            var s = separator;

            return Invariant(
                $"{d:yyyy}{s}{d:MMdd}{s}{d:yyyyMMdd}_{d:HHmmss}_{n:x8}{extension}"
            );
        }

        private static Random CreateRandom()
        {
            // Random by default uses a time-based seed, which could cause
            // collisions if two RSMassTransit instances create their Random
            // simultaneously.  To avoid this, obtain a random-ish seed from
            // some existing generator.  Guid is convenient here.

            var seed = Guid.NewGuid().GetHashCode();
            return new Random(seed);
        }

        private static int GetRandomInt32()
        {
            lock (Random)
                return Random.Next();
        }
    }
}
