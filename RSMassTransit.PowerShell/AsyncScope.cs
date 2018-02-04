// Copyright (C) 2018 (to be determined)

using System;
using System.Threading;

namespace RSMassTransit.PowerShell
{
    /// <summary>
    ///   A scope that enables synchronous code to invoke <c>Task</c>-based
    ///   asynchronous code.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     In some scenarios, a thread has a <c>SynchronizationContext</c> that
    ///     causes awaits to resume on that same thread.  This causes a deadlock
    ///     when synchronous code starts an an asynchronous <c>Task</c> and
    ///     blocks until the <c>Task</c> completes.  The deadlock occurs because
    ///     the <c>Task</c>'s awaits cannot resume on the blocked thread.
    ///     Temporarily suppressing the <c>SynchronizationContext</c> causes
    ///     awaits to resume on <c>ThreadPool</c> threads instead, avoiding the
    ///     deadlock.
    ///   </para>
    /// </remarks>
    public sealed class AsyncScope : IDisposable
    {
        private readonly SynchronizationContext _context;

        public AsyncScope()
        {
            _context = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);
        }

        void IDisposable.Dispose()
        {
            SynchronizationContext.SetSynchronizationContext(_context);
        }
    }
}
