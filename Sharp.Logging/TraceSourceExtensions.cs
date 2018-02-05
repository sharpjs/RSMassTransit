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

namespace Sharp.Logging
{
    /// <summary>
    ///   Convenience methods for <c>TraceSouce</c>-based logging.
    /// </summary>
    public static class TraceSourceExtensions
    {
        #region Critical

        /// <summary>
        ///   Writes a critical error entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceCritical(this TraceSource trace, string message)
            => trace.TraceEvent(TraceEventType.Critical, 0, message);

        /// <summary>
        ///   Writes a critical error entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceCritical(this TraceSource trace, int id, string message)
            => trace.TraceEvent(TraceEventType.Critical, id, message);

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
        public static void TraceCritical(this TraceSource trace, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Critical, 0, format, args);

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
        public static void TraceCritical(this TraceSource trace, int id, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Critical, id, format, args);

        /// <summary>
        ///   Writes a critical error entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceCritical(this TraceSource trace, Exception exception)
            => trace.TraceData(TraceEventType.Critical, 0, exception);

        /// <summary>
        ///   Writes a critical error entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceCritical(this TraceSource trace, int id, Exception exception)
            => trace.TraceData(TraceEventType.Critical, id, exception);

        #endregion
        #region Error

        /// <summary>
        ///   Writes an error entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceError(this TraceSource trace, string message)
            => trace.TraceEvent(TraceEventType.Error, 0, message);

        /// <summary>
        ///   Writes an error entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceError(this TraceSource trace, int id, string message)
            => trace.TraceEvent(TraceEventType.Error, id, message);

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
        public static void TraceError(this TraceSource trace, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Error, 0, format, args);

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
        public static void TraceError(this TraceSource trace, int id, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Error, id, format, args);

        /// <summary>
        ///   Writes an error entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceError(this TraceSource trace, Exception exception)
            => trace.TraceData(TraceEventType.Error, 0, exception);

        /// <summary>
        ///   Writes an error entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceError(this TraceSource trace, int id, Exception exception)
            => trace.TraceData(TraceEventType.Error, id, exception);

        #endregion
        #region Warning

        /// <summary>
        ///   Writes a warning entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceWarning(this TraceSource trace, string message)
            => trace.TraceEvent(TraceEventType.Warning, 0, message);

        /// <summary>
        ///   Writes a warning entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceWarning(this TraceSource trace, int id, string message)
            => trace.TraceEvent(TraceEventType.Warning, id, message);

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
        public static void TraceWarning(this TraceSource trace, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Warning, 0, format, args);

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
        public static void TraceWarning(this TraceSource trace, int id, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Warning, id, format, args);

        /// <summary>
        ///   Writes a warning entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceWarning(this TraceSource trace, Exception exception)
            => trace.TraceData(TraceEventType.Warning, 0, exception);

        /// <summary>
        ///   Writes a warning entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceWarning(this TraceSource trace, int id, Exception exception)
            => trace.TraceData(TraceEventType.Warning, id, exception);

        #endregion
        #region Information

        /// <summary>
        ///   Writes an informational entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceInformation(this TraceSource trace, string message)
            => trace.TraceInformation(message);

        /// <summary>
        ///   Writes an informational entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceInformation(this TraceSource trace, int id, string message)
            => trace.TraceEvent(TraceEventType.Information, id, message);

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
        public static void TraceInformation(this TraceSource trace, string format, params object[] args)
            => trace.TraceInformation(format, args);

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
        public static void TraceInformation(this TraceSource trace, int id, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Information, id, format, args);

        /// <summary>
        ///   Writes an informational entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceInformation(this TraceSource trace, Exception exception)
            => trace.TraceData(TraceEventType.Information, 0, exception);

        /// <summary>
        ///   Writes an informational entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceInformation(this TraceSource trace, int id, Exception exception)
            => trace.TraceData(TraceEventType.Information, id, exception);

        #endregion
        #region Verbose

        /// <summary>
        ///   Writes a verbose entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceVerbose(this TraceSource trace, string message)
            => trace.TraceEvent(TraceEventType.Verbose, 0, message);

        /// <summary>
        ///   Writes a verbose entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceVerbose(this TraceSource trace, int id, string message)
            => trace.TraceEvent(TraceEventType.Verbose, id, message);

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
        public static void TraceVerbose(this TraceSource trace, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Verbose, 0, format, args);

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
        public static void TraceVerbose(this TraceSource trace, int id, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Verbose, id, format, args);

        /// <summary>
        ///   Writes a verbose entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceVerbose(this TraceSource trace, Exception exception)
            => trace.TraceData(TraceEventType.Verbose, 0, exception);

        /// <summary>
        ///   Writes a verbose entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceVerbose(this TraceSource trace, int id, Exception exception)
            => trace.TraceData(TraceEventType.Verbose, id, exception);

        #endregion
        #region Start

        /// <summary>
        ///   Writes a start entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceStart(this TraceSource trace, string message)
            => trace.TraceEvent(TraceEventType.Start, 0, message);

        /// <summary>
        ///   Writes a start entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceStart(this TraceSource trace, int id, string message)
            => trace.TraceEvent(TraceEventType.Start, id, message);

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
        public static void TraceStart(this TraceSource trace, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Start, 0, format, args);

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
        public static void TraceStart(this TraceSource trace, int id, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Start, id, format, args);

        /// <summary>
        ///   Writes a start entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceStart(this TraceSource trace, Exception exception)
            => trace.TraceData(TraceEventType.Start, 0, exception);

        /// <summary>
        ///   Writes a start entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceStart(this TraceSource trace, int id, Exception exception)
            => trace.TraceData(TraceEventType.Start, id, exception);

        #endregion
        #region Stop

        /// <summary>
        ///   Writes a stop entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceStop(this TraceSource trace, string message)
            => trace.TraceEvent(TraceEventType.Stop, 0, message);

        /// <summary>
        ///   Writes a stop entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceStop(this TraceSource trace, int id, string message)
            => trace.TraceEvent(TraceEventType.Stop, id, message);

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
        public static void TraceStop(this TraceSource trace, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Stop, 0, format, args);

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
        public static void TraceStop(this TraceSource trace, int id, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Stop, id, format, args);

        /// <summary>
        ///   Writes a stop entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceStop(this TraceSource trace, Exception exception)
            => trace.TraceData(TraceEventType.Stop, 0, exception);

        /// <summary>
        ///   Writes a stop entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceStop(this TraceSource trace, int id, Exception exception)
            => trace.TraceData(TraceEventType.Stop, id, exception);

        #endregion
        #region Suspend

        /// <summary>
        ///   Writes a suspend entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceSuspend(this TraceSource trace, string message)
            => trace.TraceEvent(TraceEventType.Suspend, 0, message);

        /// <summary>
        ///   Writes a suspend entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceSuspend(this TraceSource trace, int id, string message)
            => trace.TraceEvent(TraceEventType.Suspend, id, message);

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
        public static void TraceSuspend(this TraceSource trace, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Suspend, 0, format, args);

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
        public static void TraceSuspend(this TraceSource trace, int id, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Suspend, id, format, args);

        /// <summary>
        ///   Writes a suspend entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceSuspend(this TraceSource trace, Exception exception)
            => trace.TraceData(TraceEventType.Suspend, 0, exception);

        /// <summary>
        ///   Writes a suspend entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceSuspend(this TraceSource trace, int id, Exception exception)
            => trace.TraceData(TraceEventType.Suspend, id, exception);

        #endregion
        #region Resume

        /// <summary>
        ///   Writes a resume entry to the log.
        /// </summary>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceResume(this TraceSource trace, string message)
            => trace.TraceEvent(TraceEventType.Resume, 0, message);

        /// <summary>
        ///   Writes a resume entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceResume(this TraceSource trace, int id, string message)
            => trace.TraceEvent(TraceEventType.Resume, id, message);

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
        public static void TraceResume(this TraceSource trace, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Resume, 0, format, args);

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
        public static void TraceResume(this TraceSource trace, int id, string format, params object[] args)
            => trace.TraceEvent(TraceEventType.Resume, id, format, args);

        /// <summary>
        ///   Writes a resume entry to the log.
        /// </summary>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceResume(this TraceSource trace, Exception exception)
            => trace.TraceData(TraceEventType.Resume, 0, exception);

        /// <summary>
        ///   Writes a resume entry to the log.
        /// </summary>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="exception">An exception to report in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceResume(this TraceSource trace, int id, Exception exception)
            => trace.TraceData(TraceEventType.Resume, id, exception);

        #endregion
        #region Operations / Correlation

        /// <summary>
        ///   Starts a logical operation, writing a start entry to the log.
        /// </summary>
        /// <param name="name">The name of the operation.</param>
        /// <returns>
        ///   An <c>TraceOperation</c> representing the logical operation.
        ///   When disposed, the <c>TraceOperation</c> writes stop and error entries to the log.
        /// </returns>
        public static TraceOperation Operation(this TraceSource trace, [CallerMemberName] string name = null)
        {
            return new TraceOperation(trace, name);
        }

        /// <summary>
        ///   Runs a logical operation, writing start, stop, and error entries to the log.
        /// </summary>
        /// <param name="name">The name of the operation.</param>
        /// <param name="action">The operation.</param>
        [DebuggerStepThrough]
        public static void Do(this TraceSource trace, string name, Action action)
        {
            TraceOperation.Do(trace, name, action);
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
        public static T Do<T>(this TraceSource trace, string name, Func<T> action)
        {
            return TraceOperation.Do(trace, name, action);
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
        public static void TraceTransfer(this TraceSource trace, int id, string message, Guid newActivityId)
            => trace.TraceTransfer(id, message, newActivityId);

        #endregion
        #region Event

        /// <summary>
        ///   Writes an entry to the log.
        /// </summary>
        /// <param name="eventType">The type of entry to write.</param>
        /// <param name="id">A numeric identifier for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceEvent(this TraceSource trace, TraceEventType eventType, int id)
            => trace.TraceEvent(eventType, id);

        /// <summary>
        ///   Writes an entry to the log.
        /// </summary>
        /// <param name="eventType">The type of entry to write.</param>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="message">A message for the entry.</param>
        [Conditional("TRACE")]
        public static void TraceEvent(this TraceSource trace, TraceEventType eventType, int id, string message)
            => trace.TraceEvent(eventType, id, message);

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
        public static void TraceEvent(this TraceSource trace, TraceEventType eventType, int id, string format, params object[] args)
            => trace.TraceEvent(eventType, id, format, args);

        #endregion
        #region Data

        /// <summary>
        ///   Writes arbitrary object data to the log.
        /// </summary>
        /// <param name="eventType">The type of event to write.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The object data to include in the event.</param>
        [Conditional("TRACE")]
        public static void TraceData(this TraceSource trace, TraceEventType eventType, int id, object data)
            => trace.TraceData(eventType, id, data);

        /// <summary>
        ///   Writes arbitrary object data to the log.
        /// </summary>
        /// <param name="eventType">The type of entry to write.</param>
        /// <param name="id">A numeric identifier for the entry.</param>
        /// <param name="data">The object data to include in the entry.</param>
        [Conditional("TRACE")]
        public static void TraceData(this TraceSource trace, TraceEventType eventType, int id, params object[] data)
            => trace.TraceData(eventType, id, data);

        #endregion
    }
}
