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

using System.Threading;
using System.Threading.Tasks;

namespace RSMassTransit.Core
{
    /// <summary>
    ///   A gate for asynchronous tasks.  When the gate is closed, tasks wait
    ///   until the gate opens.  When the gate is open, tasks proceed without
    ///   delay.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     Inspired by Stephen Toub's <c>AsyncManualResetEvent</c> article:
    ///     https://blogs.msdn.microsoft.com/pfxteam/2012/02/11/building-async-coordination-primitives-part-1-asyncmanualresetevent/
    ///   </para>
    /// </remarks>
    internal class AsyncGate
    {
        // The internal gate is a TaskCompletionSource, which functions as a
        // single-use gate.  An instance starts in the closed state.  Once
        // opened, it remains open until replaced by a new closed instance.
        private TaskCompletionSource<int> _gate;

        /// <summary>
        ///   Initializes a new instance of the <see cref="AsyncGate"/> class.
        /// </summary>
        /// <param name="open">
        ///   Whether the gate is initially open.
        /// </param>
        public AsyncGate(bool open = true)
        {
            _gate = new TaskCompletionSource<int>();

            if (open) Open();
        }

        /// <summary>
        ///   Gets or sets whether the gate is open.
        /// </summary>
        public bool IsOpen
        {
            get => IsGateOpen(_gate);
            set
            {
                if (value)
                    Open();
                else
                    Close();
            }
        }

        private static bool IsGateOpen(TaskCompletionSource<int> gate)
        {
            return gate.Task.IsCompleted;
        }

        /// <summary>
        ///   Closes the gate.
        /// </summary>
        public void Close()
        {
            // If the internal gate is still closed, there is nothing to do.
            // If it is open, replace it with a new, closed internal gate.

            var oldGate = _gate;
            if (!IsGateOpen(oldGate))
                return;

            var newGate = new TaskCompletionSource<int>();

            for (;;)
            {
                var actual = Interlocked.CompareExchange(ref _gate, newGate, oldGate);

                if (actual == oldGate)
                    // Replaced by this thread, now closed
                    return;

                if (!IsGateOpen(actual))
                    // Replaced by other thread, still closed
                    return;

                // Replaced by other thread, then opened; retry
                oldGate = actual;
            }
        }

        /// <summary>
        ///   Opens the gate.
        /// </summary>
        public void Open()
        {
            _gate.TrySetResult(default);
        }

        /// <summary>
        ///   Waits until the gate is open, asynchronously.  If the gate is
        ///   open already, there is no delay.
        /// </summary>
        /// <returns>
        ///   A task that completes when the gate is open.
        /// </returns>
        public Task WaitAsync()
        {
            return _gate.Task;
        }
    }
}
