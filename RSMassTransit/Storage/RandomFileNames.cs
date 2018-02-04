using System;

using static System.FormattableString;

namespace RSMassTransit.Storage
{
    internal static class RandomFileNames
    {
        private static readonly Random
            Random = new Random();

        internal static string Next(char separator = '.', string extension = "")
        {
            // Format: YYYY.MMDD.HHMMSSxxxxxxxxxxxxxxxx

            var d = DateTime.UtcNow;
            var n = GetRandomUInt64();
            var s = separator;

            return Invariant(
                $"{d:yyyy}{s}{d:MMdd}{s}{d:HHmmss}{n:x8}{extension}"
            );
        }

        private static ulong GetRandomUInt64()
        {
            // unsigned to avoid sign-extension
            ulong a, b;
            
            lock (Random)
            {
                a = (uint) Random.Next();
                b = (uint) Random.Next();
            }

            return (a << 32) | b;
        }
    }
}
