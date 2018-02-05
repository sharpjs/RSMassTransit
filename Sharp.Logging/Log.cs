/*
    Copyright (C) 2018 Jeffrey Sharp

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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Sharp.Logging
{
    /// <summary>
    ///   Convenience methods for <c>TraceSouce</c>-based logging.
    /// </summary>
    public abstract class Log<TProvider>
        where TProvider : ITraceSourceProvider, new()
    {
        protected Log()
        {
            throw new NotSupportedException
            (
                "This type provides static methods only. " +
                "Objects of this type cannot be created."
            );
        }

        #region TraceSource

        private static readonly TraceSource _trace
            = new TProvider().GetTraceSource();

        /// <summary>
        ///   Gets the <c>System.Diagnostics.TraceSource</c> instance to which
        ///   the static methods of this class foward invocations.
        /// </summary>
        public static TraceSource TraceSource
            => _trace;

        /// <summary>
        ///   Flushes all trace listeners attached to the log.
        /// </summary>
        public static void Flush()
            => _trace.Flush();

        /// <summary>
        ///   Closes all trace listeners attached to the log.
        /// </summary>
        public static void Close()
            => _trace.Close();

        #endregion
        #region Critical

        /// <summary>
        ///   Writes a critical error entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Critical(string message)
            => _trace.TraceCritical(message);

        /// <summary>
        ///   Writes a critical error entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Critical(int id, string message)
            => _trace.TraceCritical(id, message);

        /// <summary>
        ///   Writes a critical error entry to the log.
        /// </summary>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Critical(string format, params object[] args)
            => _trace.TraceCritical(0, format, args);

        /// <summary>
        ///   Writes a critical error entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Critical(int id, string format, params object[] args)
            => _trace.TraceCritical(id, format, args);

        /// <summary>
        ///   Writes a critical error entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Critical(Exception exception)
            => _trace.TraceCritical(0, exception);

        /// <summary>
        ///   Writes a critical error entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Critical(int id, Exception exception)
            => _trace.TraceCritical(id, exception);

        #endregion
        #region Error

        /// <summary>
        ///   Writes an error entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Error(string message)
            => _trace.TraceError(0, message);

        /// <summary>
        ///   Writes an error entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Error(int id, string message)
            => _trace.TraceError(id, message);

        /// <summary>
        ///   Writes an error entry to the log.
        /// </summary>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Error(string format, params object[] args)
            => _trace.TraceError(0, format, args);

        /// <summary>
        ///   Writes an error entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Error(int id, string format, params object[] args)
            => _trace.TraceError(id, format, args);

        /// <summary>
        ///   Writes an error entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Error(Exception exception)
            => _trace.TraceError(0, exception);

        /// <summary>
        ///   Writes an error entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Error(int id, Exception exception)
            => _trace.TraceError(id, exception);

        #endregion
        #region Warning

        /// <summary>
        ///   Writes a warning entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Warning(string message)
            => _trace.TraceWarning(0, message);

        /// <summary>
        ///   Writes a warning entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Warning(int id, string message)
            => _trace.TraceWarning(id, message);

        /// <summary>
        ///   Writes a warning entry to the log.
        /// </summary>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Warning(string format, params object[] args)
            => _trace.TraceWarning(0, format, args);

        /// <summary>
        ///   Writes a warning entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Warning(int id, string format, params object[] args)
            => _trace.TraceWarning(id, format, args);

        /// <summary>
        ///   Writes a warning entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Warning(Exception exception)
            => _trace.TraceWarning(0, exception);

        /// <summary>
        ///   Writes a warning entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Warning(int id, Exception exception)
            => _trace.TraceWarning(id, exception);

        #endregion
        #region Information

        /// <summary>
        ///   Writes an informational entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Information(string message)
            => _trace.TraceInformation(message);

        /// <summary>
        ///   Writes an informational entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Information(int id, string message)
            => _trace.TraceInformation(id, message);

        /// <summary>
        ///   Writes an informational entry to the log.
        /// </summary>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Information(string format, params object[] args)
            => _trace.TraceInformation(format, args);

        /// <summary>
        ///   Writes an informational entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Information(int id, string format, params object[] args)
            => _trace.TraceInformation(id, format, args);

        /// <summary>
        ///   Writes an informational entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Information(Exception exception)
            => _trace.TraceInformation(0, exception);

        /// <summary>
        ///   Writes an informational entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Information(int id, Exception exception)
            => _trace.TraceInformation(id, exception);

        #endregion
        #region Verbose

        /// <summary>
        ///   Writes a verbose entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Verbose(string message)
            => _trace.TraceVerbose(0, message);

        /// <summary>
        ///   Writes a verbose entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Verbose(int id, string message)
            => _trace.TraceVerbose(id, message);

        /// <summary>
        ///   Writes a verbose entry to the log.
        /// </summary>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Verbose(string format, params object[] args)
            => _trace.TraceVerbose(0, format, args);

        /// <summary>
        ///   Writes a verbose entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Verbose(int id, string format, params object[] args)
            => _trace.TraceVerbose(id, format, args);

        /// <summary>
        ///   Writes a verbose entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Verbose(Exception exception)
            => _trace.TraceVerbose(0, exception);

        /// <summary>
        ///   Writes a verbose entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Verbose(int id, Exception exception)
            => _trace.TraceVerbose(id, exception);

        #endregion
        #region Start

        /// <summary>
        ///   Writes a start entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Start(string message)
            => _trace.TraceStart(0, message);

        /// <summary>
        ///   Writes a start entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Start(int id, string message)
            => _trace.TraceStart(id, message);

        /// <summary>
        ///   Writes a start entry to the log.
        /// </summary>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Start(string format, params object[] args)
            => _trace.TraceStart(0, format, args);

        /// <summary>
        ///   Writes a start entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Start(int id, string format, params object[] args)
            => _trace.TraceStart(id, format, args);

        /// <summary>
        ///   Writes a start entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Start(Exception exception)
            => _trace.TraceStart(0, exception);

        /// <summary>
        ///   Writes a start entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Start(int id, Exception exception)
            => _trace.TraceStart(id, exception);

        #endregion
        #region Stop

        /// <summary>
        ///   Writes a stop entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Stop(string message)
            => _trace.TraceStop(0, message);

        /// <summary>
        ///   Writes a stop entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Stop(int id, string message)
            => _trace.TraceStop(id, message);

        /// <summary>
        ///   Writes a stop entry to the log.
        /// </summary>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Stop(string format, params object[] args)
            => _trace.TraceStop(0, format, args);

        /// <summary>
        ///   Writes a stop entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Stop(int id, string format, params object[] args)
            => _trace.TraceStop(id, format, args);

        /// <summary>
        ///   Writes a stop entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Stop(Exception exception)
            => _trace.TraceStop(0, exception);

        /// <summary>
        ///   Writes a stop entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Stop(int id, Exception exception)
            => _trace.TraceStop(id, exception);

        #endregion
        #region Suspend

        /// <summary>
        ///   Writes a suspend entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Suspend(string message)
            => _trace.TraceSuspend(0, message);

        /// <summary>
        ///   Writes a suspend entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Suspend(int id, string message)
            => _trace.TraceSuspend(id, message);

        /// <summary>
        ///   Writes a suspend entry to the log.
        /// </summary>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Suspend(string format, params object[] args)
            => _trace.TraceSuspend(0, format, args);

        /// <summary>
        ///   Writes a suspend entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Suspend(int id, string format, params object[] args)
            => _trace.TraceSuspend(id, format, args);

        /// <summary>
        ///   Writes a suspend entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Suspend(Exception exception)
            => _trace.TraceSuspend(0, exception);

        /// <summary>
        ///   Writes a suspend entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Suspend(int id, Exception exception)
            => _trace.TraceSuspend(id, exception);

        #endregion
        #region Resume

        /// <summary>
        ///   Writes a resume entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Resume(string message)
            => _trace.TraceResume(0, message);

        /// <summary>
        ///   Writes a resume entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Resume(int id, string message)
            => _trace.TraceResume(id, message);

        /// <summary>
        ///   Writes a resume entry to the log.
        /// </summary>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Resume(string format, params object[] args)
            => _trace.TraceResume(0, format, args);

        /// <summary>
        ///   Writes a resume entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Resume(int id, string format, params object[] args)
            => _trace.TraceResume(id, format, args);

        /// <summary>
        ///   Writes a resume entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Resume(Exception exception)
            => _trace.TraceResume(0, exception);

        /// <summary>
        ///   Writes a resume entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void Resume(int id, Exception exception)
            => _trace.TraceResume(id, exception);

        #endregion
        #region Operations / Correlation

        /// <summary>
        ///   Gets the identifier of the current logical activity.
        /// </summary>
        public static Guid ActivityId
            => Trace.CorrelationManager.ActivityId;

        /// <summary>
        ///   Gets the current stack of logial operations.
        /// </summary>
        /// <returns>
        ///   A new array containing the objects in the logical operation stack,
        ///   ordered from top to bottom.
        /// </returns>
        public static object[] GetOperationStack()
            => Trace.CorrelationManager.LogicalOperationStack.ToArray();

        /// <summary>
        ///   Starts a logical operation, writing a start entry to the log.
        /// </summary>
        /// <param name="name">The name of the operation.</param>
        /// <returns>
        ///   An <c>TraceOperation</c> representing the logical operation.
        ///   When disposed, the <c>TraceOperation</c> writes stop and error entries to the log.
        /// </returns>
        public static TraceOperation Operation([CallerMemberName] string name = null)
        {
            return new TraceOperation(_trace, name);
        }

        /// <summary>
        ///   Runs a logical operation, writing start, stop, and error entries to the log.
        /// </summary>
        /// <param name="name">The name of the operation.</param>
        /// <param name="action">The operation.</param>
        [DebuggerStepThrough]
        public static void Do(string name, Action action)
        {
            TraceOperation.Do(_trace, name, action);
        }

        /// <summary>
        ///   Runs a logical operation, writing start, stop, and error entries to the log.
        /// </summary>
        /// <param name="name">The name of the operation.</param>
        /// <param name="action">The operation.</param>
        [DebuggerStepThrough]
        public static Task DoAsync(string name, Func<Task> action)
        {
            return TraceOperation.DoAsync(_trace, name, action);
        }

        /// <summary>
        ///   Runs a logical operation, writing start, stop, and error entries to the log.
        /// </summary>
        /// <param name="name">The name of the operation.</param>
        /// <param name="action">The operation.</param>
        /// <returns>
        ///   The value returned by <paramref name="action"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static T Do<T>(string name, Func<T> action)
        {
            return TraceOperation.Do(_trace, name, action);
        }

        /// <summary>
        ///   Runs a logical operation, writing start, stop, and error entries to the log.
        /// </summary>
        /// <param name="name">The name of the operation.</param>
        /// <param name="action">The operation.</param>
        /// <returns>
        ///   The value returned by <paramref name="action"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static Task<T> DoAsync<T>(string name, Func<Task<T>> action)
        {
            return TraceOperation.DoAsync(_trace, name, action);
        }

        /// <summary>
        ///   Writes an entry to the log, reporting a new identifier for the current logical activity.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        /// <param name="newActivityId">A new identifier for the current logical activity.</param>
        /// <remarks>
        ///   <c>Transfer</c> is intended to be used with the logical activities of a
        ///   <c>CorrelationManager</c>. The <paramref name="newActivityId"/> parameter
        ///   relates to the <c>ActivityId</c> property of a <c>CorrelationManager</c> object.
        ///   If a logical operation begins in one activity and transfers to another,
        ///   the second activity should log the transfer by calling the <c>Transfer</c> method.
        ///   The <c>Transfer</c> call relates the new activity identifier to the previous one.
        ///   The most likely consumer of this functionality is a trace viewer that can report
        ///   logical operations that span multiple activities.
        /// </remarks>
        [Conditional("TRACE")]
        public static void Transfer(int id, string message, Guid newActivityId)
            => _trace.TraceTransfer(id, message, newActivityId);

        #endregion
        #region Event

        /// <summary>
        ///   Writes an entry to the log.
        /// </summary>
        /// <param name="eventType">The type of entry to write.</param>
        /// <param name="id">A numeric identifier for the entry.</param>
        [Conditional("TRACE")]
        public static void Event(TraceEventType eventType, int id)
            => _trace.TraceEvent(eventType, id);

        /// <summary>
        ///   Writes an entry to the log.
        /// </summary>
        /// <param name="eventType">The type of entry to write.</param>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void Event(TraceEventType eventType, int id, string message)
            => _trace.TraceEvent(eventType, id, message);

        /// <summary>
        ///   Writes an entry to the log.
        /// </summary>
        /// <param name="eventType">The type of entry to write.</param>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="format">A format string to build a message for the entry.</param>
        /// <param name="args">The objects to substitute into the format string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format"/> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///   <paramref name="format"/> is invalid or
        ///   specifies an argument position not present in <paramref name="args"/>.
        /// </exception>
        [Conditional("TRACE")]
        public static void Event(TraceEventType eventType, int id, string format, params object[] args)
            => _trace.TraceEvent(eventType, id, format, args);

        #endregion
        #region Data

        /// <summary>
        ///   Writes arbitrary object data to the log.
        /// </summary>
        /// <param name="eventType">The type of event to write.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The object data to include in the event.</param>
        [Conditional("TRACE")]
        public static void Data(TraceEventType eventType, int id, object data)
            => _trace.TraceData(eventType, id, data);

        /// <summary>
        ///   Writes arbitrary object data to the log.
        /// </summary>
        /// <param name="eventType">The type of entry to write.</param>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="data">The object data to include in the entry.</param>
        [Conditional("TRACE")]
        public static void Data(TraceEventType eventType, int id, params object[] data)
            => _trace.TraceData(eventType, id, data);

        #endregion
        #region Event Handlers

        /// <summary>
        ///   Enables logging of all thrown exceptions.
        /// </summary>
        /// <remarks>
        ///   This method causes ALL exceptions to be logged, even the caught
        ///   exceptions.  This results in an extremely noisy log.  Use only for
        ///   debugging, and then only when you are desperate.
        /// </remarks>
        public static void ConfigureLogAllThrownExceptions()
        {
            AppDomain.CurrentDomain.FirstChanceException += AppDomain_FirstChanceException;
        }

        /// <summary>
        ///   Enables handlers that log application termination and close all
        ///   attached listeners.
        /// </summary>
        public static void ConfigureCloseOnExit()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;
            AppDomain.CurrentDomain.DomainUnload       += AppDomain_DomainUnload;
            AppDomain.CurrentDomain.ProcessExit        += AppDomain_ProcessExit;
        }

        // Used to detect and avoid reentrancy in the first-chance exception handler.
        // Type is int because Interlocked.CompareExchange does not support bool.
        private static int _inEvent;

        [SecurityCritical, HandleProcessCorruptedStateExceptions]
        // ^^ These attributes opt-in this handler to receive certain severe
        //    exceptions that indicate the process state might be corrupt, such
        //    as stack overflows and access violations.
        private static void AppDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            // Raised when ANY exception is thrown, either by app code or by
            // framework code, and BEFORE any catch.  These are called
            // 'first-chance' exceptions.  Some will be caught and handled, and
            // as such are not necessarily problems the user needs to know
            // about.  They are observed here for diagnostic purposes.

            // This is critical code; make no assumptions.
            var exception = e?.Exception;
            if (exception == null)
                return;

            // Avoid reentrancy if this method causes its own exceptions.
            // That would result in a stack overflow.
            if (Interlocked.CompareExchange(ref _inEvent, -1, 0) != 0)
                return;

            try
            {
                Verbose(
                    "An exception was thrown of type {0}: {1}",
                    exception.GetType().FullName,
                    exception.ToString()
                );

                Verbose(exception);
            }
            catch
            {
                // Logging here is best-effort only.  If the logging API throws
                // an exception here, there is no choice but to ignore it.  The
                // original exception that fired this event must be allowed to
                // continue, so it can be caught and handled.
            }

            Interlocked.Exchange(ref _inEvent, 0);
        }

        [SecurityCritical, HandleProcessCorruptedStateExceptions]
        // ^^ These attributes opt-in this handler to receive certain severe
        //    exceptions that indicate the process state might be corrupt, such
        //    as stack overflows and access violations.
        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Raised when neither app code nor any framework code handles an
            // exception, except for a few special cases which go unnoticed:
            //
            // * ThreadAbortException when the thread's Abort method is invoked
            // * AppDomainUnloadedException when the thread's application domain
            //     is being unloaded
            // * CLR internal exceptions

            // This is critical code; make no assumptions.
            var exceptionObject = e?.ExceptionObject;
            if (exceptionObject == null)
                return;

            var type      = exceptionObject.GetType().FullName;
            var exception = exceptionObject as Exception;

            if (e.IsTerminating)
            {
                Critical("Terminating due to an unhandled exception of type {0}.", type);

                if (exception != null)
                    Critical(exception);

                Close();
            }
            else
            {
                Error("Unhandled exception of type {0}.  Execution will continue.", type);

                if (exception != null)
                    Error(exception);
            }
        }

        private static void AppDomain_DomainUnload(object sender, EventArgs e)
        {
            // Raised in a non-default application domain when its Unload method
            // is invoked.  NEVER raised in the default application domain.

            Information("The AppDomain is unloading.");
            Close();
        }

        private static void AppDomain_ProcessExit(object sender, EventArgs e)
        {
            // Raised in every application domain when the hosting process is
            // exiting normally.

            Information("The AppDomain's parent process is exiting.");
            Close();
        }

        #endregion
    }
}
