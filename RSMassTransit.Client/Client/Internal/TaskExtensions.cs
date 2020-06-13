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
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace RSMassTransit.Client.Internal
{
    internal static class TaskExtensions
    {
        /// <summary>
        ///   Waits for the task to complete execution.
        ///   If the task throws an <c>AggregateException</c> wrapping a single
        ///   inner exception, this method rethrows the inner exception.
        /// </summary>
        /// <param name="task">The task to await.</param>
        public static void WaitOrThrowUnwrapped(this Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            try
            {
                task.Wait();
            }
            catch (AggregateException e)
            {
                if (e.GetBaseExceptions().TrySingle(out Exception inner))
                    ExceptionDispatchInfo.Capture(inner).Throw();

                throw;
            }
        }

        /// <summary>
        ///   Waits for the task to complete execution and returns its result.
        ///   If the task throws an <c>AggregateException</c> wrapping a single
        ///   inner exception, this method rethrows the inner exception.
        /// </summary>
        /// <param name="task">The task to await.</param>
        public static T GetResultOrThrowUnwrapped<T>(this Task<T> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            try
            {
                return task.Result;
            }
            catch (AggregateException e)
            {
                if (e.GetBaseExceptions().TrySingle(out Exception inner))
                    ExceptionDispatchInfo.Capture(inner).Throw();

                throw;
            }
        }
    }
}
