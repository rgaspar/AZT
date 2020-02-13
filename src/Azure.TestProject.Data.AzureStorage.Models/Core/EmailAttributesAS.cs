using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Data.AzureStorage.Models.Core
{
    public class EmailAttributesAS : TableEntity
    {
        #region properties
        public DateTime CreatedDateUtc { get; set; }
        public string Key { get; set; }
        public string Email { get; set; }
        public string Attribute { get; set; }
        #endregion

        public void AssignRowKey()
        {
            this.RowKey = CreatedDateUtc.ToString();
        }
        public void AssignPartitionKey()
        {
            this.PartitionKey = Key;
        }
    }
}
