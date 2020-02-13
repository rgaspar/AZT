using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common.DependencyInjection
{
    public class AssemblyInfo : IComparable<AssemblyInfo>, IComparable
    {
        private const int ThisIsGreaterThanThat = 1;

        public AssemblyInfo(Assembly assembly)
        {
            Assembly = assembly;
            FullName = assembly.FullName;
            Name = assembly.GetName().Name;
        }

        public string Name { get; }

        public string FullName { get; }

        public Assembly Assembly { get; }

        public int CompareTo(object obj)
        {
            return obj is AssemblyInfo assemblyInfo ? CompareTo(assemblyInfo) : ThisIsGreaterThanThat;
        }

        public int CompareTo(AssemblyInfo other)
        {
            return other is null ? ThisIsGreaterThanThat : FullName.CompareTo(other.FullName);
        }
    }
}
