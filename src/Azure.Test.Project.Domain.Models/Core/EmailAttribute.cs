using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Test.Project.Domain.Models.Core
{
    public class EmailAttribute
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Email { get; set; }

        public string Attribute { get; set; }
    }
}
