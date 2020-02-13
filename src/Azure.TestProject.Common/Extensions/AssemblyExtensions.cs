using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common.Extensions
{
    public static class AssemblyExtensions
    {
        public static string SafeGetLocation(this Assembly assembly)
        {
            if (assembly is null)
            {
                return null;
            }

            try
            {
                return assembly.Location;
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }

        public static string SafeGetCodeBase(this Assembly assembly)
        {
            if (assembly is null)
            {
                return null;
            }

            try
            {
                return assembly.CodeBase;
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }
    }
}
