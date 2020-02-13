using Azure.TestProject.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Data.Models
{
    public abstract class Auditable : IAuditable
    {
        public DateTimeOffset CreatedAtUtc { get; set; }
    }
}
