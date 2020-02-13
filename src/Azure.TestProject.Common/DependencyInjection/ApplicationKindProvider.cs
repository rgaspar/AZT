using Azure.TestProject.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common.DependencyInjection
{
    public static class ApplicationKindProvider
    {
        private static readonly ExecutionCounter counter = new ExecutionCounter(1);

        public static ApplicationKind Current { get; private set; } = ApplicationKind.Unknown;

        public static void Initialize(ApplicationKind applicationKind)
        {
            counter.ThrowIfCannotExecute(typeof(ApplicationKindProvider));

            ThrowIfInvalid(applicationKind);

            Current = applicationKind;
        }

        private static void ThrowIfInvalid(ApplicationKind applicationKind)
        {
            if (!Enum.IsDefined(typeof(ApplicationKind), applicationKind))
            {
                throw new ArgumentException(
                    $"Invalid parameter value. [{nameof(applicationKind)}] = [{applicationKind}]",
                    nameof(applicationKind)
                );
            }
        }
    }
}
