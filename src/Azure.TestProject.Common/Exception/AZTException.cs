using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common
{
    public class AZTException : Exception
    {
        public AZTException()
            : this("The requested operation failed.", null, null)
        {
        }

        public AZTException(string message)
            : this(message, null, null)
        {
        }

        public AZTException(string message, HttpStatusCode httpStatusCode)
            : this(message, null, httpStatusCode)
        {
        }

        public AZTException(string message, Exception innerException)
            : this(message, innerException, null)
        {
        }

        public AZTException(string message, Exception innerException, HttpStatusCode? httpStatusCode)
            : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode? HttpStatusCode { get; }
    }
}
