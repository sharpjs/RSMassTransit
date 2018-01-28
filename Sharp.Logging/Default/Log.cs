// Copyright (C) 2018 (to be determined)

namespace Sharp.Logging.Default
{
    /// <summary>
    ///   Ready-to-use logging API.  Writes to a <c>TraceSource</c> named 'Log'.
    /// </summary>
    public abstract class Log : Log<DefaultTraceSourceProvider> { }
}
