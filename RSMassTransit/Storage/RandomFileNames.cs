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
            // Format: YYYY.MMDD.HHMMSSxxxxxxxxxxxxxxxx

            var d = DateTime.UtcNow;
            var n = GetRandomUInt64();
            var s = separator;

            return Invariant(
                $"{d:yyyy}{s}{d:MMdd}{s}{d:HHmmss}{n:x8}{extension}"
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
