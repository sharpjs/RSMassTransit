// Copyright (C) 2018 (to be determined)

using System.Diagnostics;

namespace Sharp.Logging
{
    /// <summary>
    ///   Provides the <c>TraceSource</c> object used by <see cref="Log{TProvider}"/>.
    /// </summary>
    public interface ITraceSourceProvider
    {
        /// <summary>
        ///   Gets the <c>TraceSource</c> object for this provider.
        /// </summary>
        /// <remarks>
        ///   This method is invoked by <see cref="Log{TProvider}"/> only once
        ///   for a particular <c>TProvider</c> type argument.
        /// </remarks>
        /// <returns>
        ///   A <c>TraceSource</c> object.
        /// </returns>
        TraceSource GetTraceSource();
    }
}
