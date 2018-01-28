// Copyright (C) 2018 (to be determined)

using System.Diagnostics;

namespace Sharp.Logging.Default
{
    /// <summary>
    ///   A ready-to-use <see cref="ITraceSourceProvider"/> that provides a
    ///   <c>TraceSource</c> named 'Log'.
    /// </summary>
    public class DefaultTraceSourceProvider : ITraceSourceProvider
    {
        /// <inheritdoc/>
        public TraceSource GetTraceSource() => new TraceSource("Log");
    }
}
