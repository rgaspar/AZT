using Azure.TestProject.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Data.Models.Core
{
    public class EmailAttribute : Auditable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Key { get; set; }

        [Required]
        [StringLength(128)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Attribute { get; set; }
    }
}
