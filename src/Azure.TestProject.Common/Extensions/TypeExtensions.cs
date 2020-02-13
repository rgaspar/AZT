using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common
{
    public static class TypeExtensions
    {
        public static class GenericTypeDefinition
        {
            public static readonly Type IEnumerable = typeof(IEnumerable<>);
        }

        public static bool IsIEnumerable(this Type type)
        {
            bool isIEnumerable =
                type.IsInterface
                &&
                type.IsGenericType
                &&
                type.GetGenericTypeDefinition() == GenericTypeDefinition.IEnumerable;

            if (!isIEnumerable)
            {
                isIEnumerable = type.GetInterfaces().Any(interfaceType => interfaceType.IsIEnumerable());
            }

            return isIEnumerable;
        }
    }
}
