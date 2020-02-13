using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common
{
    public abstract class ExceptionHandler<TException> : ExceptionHandler
        where TException : Exception
    {
        public abstract AZTException CreateAZTException(TException ex);

        public sealed override AZTException CreateAZTException(Exception ex)
        {
            return CreateAZTException((TException)ex);
        }
    }

    public abstract class ExceptionHandler
    {
        public abstract AZTException CreateAZTException(Exception ex);
    }
}
