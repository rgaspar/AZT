using Autofac;
using Autofac.Builder;
using Azure.TestProject.Common.Enumerations;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Azure.TestProject.Common.DependencyInjection
{
    public static class AutofacRegistrationExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InstancePerApplicationKind<TLimit, TActivatorData, TStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration
        )
        {
            if (registration is null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            LogType<TLimit>();

            IRegistrationBuilder<TLimit, TActivatorData, TStyle> result;

            switch (ApplicationKindProvider.Current)
            {
                case ApplicationKind.AspNetWebApi2:
                    result = registration.InstancePerRequest();
                    break;

                case ApplicationKind.AspNetCore:
                    result = registration.InstancePerLifetimeScope();
                    break;

                case ApplicationKind.Unknown:
                default:
                    throw new InvalidOperationException(
                        $"{nameof(ApplicationKindProvider)}.{nameof(ApplicationKindProvider.Initialize)}({nameof(ApplicationKind)}) method must be called before registering dependencies."
                    );
            }

            return result;
        }

        [Conditional("DEBUG")]
        private static void LogType<TType>()
        {
            Type dependencyType = typeof(TType);

            Log($"Registering dependency: {nameof(ApplicationKind)}=[{ApplicationKindProvider.Current}] Type=[{dependencyType.FullName ?? dependencyType.Name}]");
        }

        [Conditional("DEBUG")]
        private static void Log(string message, [CallerMemberName] string callerMethodName = "(unknown method)")
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(message, callerMethodName);
            }
        }
    }
}
