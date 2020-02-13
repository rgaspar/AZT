using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common
{
    public static class HexadecimalFormats
    {
        public const string LowerCaseForByte = "x2";

        public const string UpperCaseForByte = "X2";

        public static string GetFormatString(bool upperCase = false, int length = 2)
        {
            return $"{(upperCase ? "X" : "x")}{length}";
        }
    }
}
