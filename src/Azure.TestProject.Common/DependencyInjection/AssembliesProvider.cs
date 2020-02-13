using Azure.TestProject.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common.DependencyInjection
{
    public static class AssembliesProvider
    {
        private const string RootNamespace = "Azure.TestProject.";

        private static readonly Dictionary<string, SortedSet<AssemblyInfo>> assemblies;

        static AssembliesProvider()
        {
            assemblies = new Dictionary<string, SortedSet<AssemblyInfo>>();

            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

        }

        public static Assembly[] GetAssemblies()
        {
            Assembly[] aztAssemblies =
                assemblies
                    .Where(kvp => kvp.Key.StartsWith(RootNamespace))
                    .Select(kvp => kvp.Value.Last().Assembly)
                    .ToArray();

            foreach (Assembly assembly in aztAssemblies)
            {
                Log($"Assembly: FullName=[{assembly.FullName}] Location=[{assembly.SafeGetLocation()}]");
            }

            return aztAssemblies;
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            Assembly assembly = args.LoadedAssembly;

            Log($"LoadedAssembly.FullName = [{assembly?.FullName ?? "(unknown)"}]");
            Log($"LoadedAssembly.Location = [{assembly.SafeGetLocation() ?? "(unknown)"}]");
            Log($"LoadedAssembly.CodeBase = [{assembly.SafeGetCodeBase() ?? "(unknown)"}]");

            var assemblyInfo = new AssemblyInfo(assembly);

            if (assemblies.TryGetValue(assemblyInfo.Name, out SortedSet<AssemblyInfo> assemblyInfoCollection))
            {
                if (assemblyInfoCollection.All(ai => ai.FullName != assemblyInfo.FullName))
                {
                    assemblyInfoCollection.Add(assemblyInfo);
                }
            }
            else
            {
                assemblyInfoCollection = new SortedSet<AssemblyInfo>() { assemblyInfo };
                assemblies[assemblyInfo.Name] = assemblyInfoCollection;
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string requestedAssemblyFullName = args.Name;

            string requestedAssemblyName =
                requestedAssemblyFullName
                    .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                    .First();

            Log($"ResolvedAssembly.Name     = [{requestedAssemblyName}]");
            Log($"ResolvedAssembly.FullName = [{requestedAssemblyFullName}]");

            Assembly assembly = null;

            if (assemblies.TryGetValue(requestedAssemblyName, out SortedSet<AssemblyInfo> assemblyInfoCollection))
            {
                assembly =
                    assemblyInfoCollection
                        .SingleOrDefault(assemblyInfo => assemblyInfo.FullName == requestedAssemblyFullName)
                        ?.Assembly // matching version of an assembly
                        ?? assemblyInfoCollection.Last().Assembly; // latest version of an assembly
            }

            Log($"ResolvedAssembly.Location = [{assembly.SafeGetLocation() ?? "(null)"}]");
            Log($"ResolvedAssembly.CodeBase = [{assembly.SafeGetCodeBase() ?? "(null)"}]");

            return assembly;
        }

        [Conditional("DEBUG")]
        private static void Log(string message, [CallerMemberName] string callerMemberName = "(unknown caller method)")
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(message, callerMemberName);
            }
        }
    }
}
