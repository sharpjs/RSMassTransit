using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace RSMassTransit.PowerShell
{
    internal static class TaskExtensions
    {
        public static T GetResultOrThrowUnwrapped<T>(this Task<T> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            try
            {
                return task.Result;
            }
            catch (AggregateException e)
            {
                if (e.GetBaseExceptions().TrySingle(out Exception inner))
                    ExceptionDispatchInfo.Capture(inner).Throw();

                throw;
            }
        }
    }
}
