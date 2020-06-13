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
using System.Threading.Tasks;
using NUnit.Framework;

namespace RSMassTransit.Client.Internal
{
    [TestFixture]
    public class TaskExtensionsTests
    {
        [Test]
        public void Wait()
        {
            Assert.Throws<AggregateException>(() =>
            {
                Task.Run(ThrowingAction).Wait();
            });
        }

        [Test]
        public void WaitOrThrowUnwrapped_Single()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Task.Run(ThrowingAction).WaitOrThrowUnwrapped();
            });
        }

        [Test]
        public void WaitOrThrowUnwrapped_Multiple()
        {
            Assert.Throws<AggregateException>(() =>
            {
                Task.WhenAll(
                    Task.Run(ThrowingAction),
                    Task.Run(ThrowingAction)
                )
                .WaitOrThrowUnwrapped();
            });
        }

        [Test]
        public void GetResult()
        {
            Assert.Throws<AggregateException>(() =>
            {
                var _ = Task.Run(ThrowingFunc).Result;
            });
        }

        [Test]
        public void GetResultOrThrowUnwrapped_Single()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var _ = Task.Run(ThrowingFunc).GetResultOrThrowUnwrapped();
            });
        }

        [Test]
        public void GetResultOrThrowUnwrapped_Multiple()
        {
            Assert.Throws<AggregateException>(() =>
            {
                var _ = Task.WhenAll(
                    Task.Run(ThrowingFunc),
                    Task.Run(ThrowingFunc)
                )
                .GetResultOrThrowUnwrapped();
            });
        }

        private static Action ThrowingAction = () =>
        {
            throw new InvalidOperationException("Uh-oh!");
        };

        private static Func<int> ThrowingFunc = () =>
        {
            throw new InvalidOperationException("Uh-oh!");
        };
    }
}
