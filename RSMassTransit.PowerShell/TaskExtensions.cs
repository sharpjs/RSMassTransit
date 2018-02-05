﻿/*
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
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace RSMassTransit.PowerShell
{
    internal static class TaskExtensions
    {
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
