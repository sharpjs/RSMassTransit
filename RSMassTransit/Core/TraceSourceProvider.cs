// Copyright (C) 2018 (to be determined)

using System.Diagnostics;
using Sharp.Logging;

namespace RSMassTransit.Core
{
    internal class TraceSourceProvider : ITraceSourceProvider
    {
        public TraceSource GetTraceSource()
            => new TraceSource("RSMassTransit");
    }
}
