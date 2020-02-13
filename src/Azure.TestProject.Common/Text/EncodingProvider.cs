using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common
{
    public static class EncodingProvider
    {
        public static Encoding UTF8WithoutBOM { get; } = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    }
}
