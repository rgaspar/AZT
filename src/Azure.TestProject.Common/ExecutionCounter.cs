using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.TestProject.Common
{
    public class ExecutionCounter
    {
        private int currentExecutionCount = 0;

        public ExecutionCounter(int maxExecutionCount)
        {
            MaxExecutionCount = maxExecutionCount;
            SetTimesString();
        }

        private int MaxExecutionCount { get; }

        private string TimesString { get; set; }

        public void ThrowIfCannotExecute<TType>([CallerMemberName] string methodName = "..unknown")
        {
            ThrowIfCannotExecute(typeof(TType), methodName);
        }

        public void ThrowIfCannotExecute(Type type, [CallerMemberName] string methodName = "..unknown")
        {
            if (CanExecute())
            {
                return;
            }

            throw new InvalidOperationException(
                $"{type?.FullName ?? type?.Name ?? "'UnknownType"}.{methodName}() method can be called only {TimesString}."
            );
        }

        private bool CanExecute()
        {
            int newExecutionCount = Interlocked.Increment(ref currentExecutionCount);

            return newExecutionCount <= MaxExecutionCount;
        }

        private void SetTimesString()
        {
            switch (MaxExecutionCount)
            {
                case 1:
                    TimesString = "once";
                    break;

                case 2:
                    TimesString = "twice";
                    break;

                case int value when (value > 2):
                    TimesString = $"{value} times";
                    break;

                default:
                    TimesString = "undefined times";
                    break;
            }
        }
    }
}
