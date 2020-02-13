using Azure.TestProject.Data.Models;
using Azure.TestProject.Data.Models.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Data
{
    public class AZTDataContext : DbContext
    {
        public AZTDataContext()
            : this("name=AzureSQL")
        {
        }

        public AZTDataContext(string connectionString)
            : base(connectionString)
        {
            Initialize();
        }

        public AZTDataContext(DbConnection connection)
           : base(connection, contextOwnsConnection: false)
        {
            Initialize();
        }

        #region Core
        public DbSet<EmailAttribute> EmailAttributes { get; set; }
        #endregion

        private void Initialize()
        {
            SetDatabaseLogger();

            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
        }

        [Conditional("DEBUG")]
        private void SetDatabaseLogger()
        {
            if (Debugger.IsAttached)
            {
                Database.Log = sqlStatement => Debug.WriteLine(sqlStatement);
            }
        }
    }
}
