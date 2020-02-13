using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common.Contracts
{
    public interface IAuditable
    {
        DateTimeOffset CreatedAtUtc { get; set; }
    }
}
