// Copyright (C) 2018 (to be determined)

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sharp.Logging
{
    public sealed class TraceOperation : IDisposable
    {
        private readonly TraceSource _trace;
        private readonly string      _name;
        private readonly DateTime    _start;
        private Exception            _exception;

        public TraceOperation([CallerMemberName] string name = null)
            : this(null, name) { }

        public TraceOperation(TraceSource trace, [CallerMemberName] string name = null)
        {
            _trace = trace;
            _name  = name;
            if (Trace.CorrelationManager.LogicalOperationStack.Count == 0)
                Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            Trace.CorrelationManager.StartLogicalOperation();
            TraceStarting();
            _start = DateTime.UtcNow;
        }

        private void Dispose()
        {
            TraceCompleted();
            Trace.CorrelationManager.StopLogicalOperation();
            if (Trace.CorrelationManager.LogicalOperationStack.Count == 0)
                Trace.CorrelationManager.ActivityId = Guid.Empty;
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        public string Name
        {
            get { return _name; }
        }

        public DateTime StartTime
        {
            get { return _start; }
        }

        public TimeSpan ElapsedTime
        {
            get { return DateTime.UtcNow - _start; }
        }

        public Exception Exception
        {
            get { return _exception; }
            set { _exception = value; }
        }

        [DebuggerStepThrough]
        public static void Do(string name, Action action)
        {
            Do(null, name, action);
        }

        [DebuggerStepThrough]
        public static void Do(TraceSource trace, string name, Action action)
        {
            var operation = new TraceOperation(trace, name);
            try
            {
                action();
            }
            catch (Exception e)
            {
                operation.Exception = e;
                throw;
            }
            finally
            {
                operation.Dispose();
            }
        }

        [DebuggerStepThrough]
        public static async Task DoAsync(TraceSource trace, string name, Func<Task> action)
        {
            var operation = new TraceOperation(trace, name);
            try
            {
                await action();
            }
            catch (Exception e)
            {
                operation.Exception = e;
                throw;
            }
            finally
            {
                operation.Dispose();
            }
        }

        [DebuggerStepThrough]
        public static TResult Do<TResult>(string name, Func<TResult> action)
        {
            return Do(null, name, action);
        }

        [DebuggerStepThrough]
        public static TResult Do<TResult>(TraceSource trace, string name, Func<TResult> action)
        {
            var operation = new TraceOperation(trace, name);
            try
            {
                return action();
            }
            catch (Exception e)
            {
                operation.Exception = e;
                throw;
            }
            finally
            {
                operation.Dispose();
            }
        }

        [DebuggerStepThrough]
        public static async Task<TResult> DoAsync<TResult>(TraceSource trace, string name, Func<Task<TResult>> action)
        {
            var operation = new TraceOperation(trace, name);
            try
            {
                return await action();
            }
            catch (Exception e)
            {
                operation.Exception = e;
                throw;
            }
            finally
            {
                operation.Dispose();
            }
        }

        private void TraceStarting()
        {
            if (_name == null)
                return;

            var trace = _trace;
            if (trace != null)
                trace.TraceInformation(_name + ": Starting");
            else
                Trace.TraceInformation(_name + ": Starting");
        }

        private void TraceCompleted()
        {
            if (_name == null)
                return;

            var time      = ElapsedTime.TotalSeconds;
            var exception = Exception;
            var trace     = _trace;
            string notice;

            if (exception != null)
            {
                if (trace != null)
                    trace.TraceEvent(TraceEventType.Error, 0, exception.ToString());
                else
                    Trace.TraceError(exception.ToString());

                notice = " [EXCEPTION]";
            }
            else
            {
                notice = "";
            }

            if (trace != null)
                trace.TraceInformation("{0}: Completed [{1:N3}s]{2}", _name, time, notice);
            else
                Trace.TraceInformation("{0}: Completed [{1:N3}s]{2}", _name, time, notice);
        }
    }
}
