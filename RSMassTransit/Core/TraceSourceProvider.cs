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
