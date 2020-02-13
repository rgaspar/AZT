using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.DataTransfer.Core
{
    public class EmailAttribute
    {
        public string Key { get; set; }

        public string Email { get; set; }

        public string[] Attributes { get; set; }
    }
}
