using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Data
{    
    internal class AZTDbInitializer : MigrateDatabaseToLatestVersion<AZTDataContext, Configuration>, IDatabaseInitializer<AZTDataContext>
    {
        private readonly DbMigrationsConfiguration configuration;

        private readonly bool useSuppliedContext;

        //static OloDbInitializer()
        //{
        //    EnsureLoadedForContext();
        //}

        public AZTDbInitializer()
            : this(useSuppliedContext: false)
        {
        }

        public AZTDbInitializer(bool useSuppliedContext)
            : this(useSuppliedContext, new Configuration())
        {
        }

        public AZTDbInitializer(bool useSuppliedContext, Configuration configuration)
            : base(useSuppliedContext, configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
            this.useSuppliedContext = useSuppliedContext;
        }

        public AZTDbInitializer(string connectionStringName)
            : base(connectionStringName)
        {
            if (String.IsNullOrWhiteSpace(connectionStringName))
            {
                throw new ArgumentException("Invalid connection string name.", nameof(connectionStringName));
            }

            var configuration = new Configuration()
            {
                TargetDatabase = new DbConnectionInfo(connectionStringName)
            };

            this.configuration = configuration;
        }

        public override void InitializeDatabase(AZTDataContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            base.InitializeDatabase(context);

            //DbMigrator dbMigrator = CreateDbMigrator(configuration, useSuppliedContext ? context : null);

            //if (dbMigrator.GetPendingMigrations().Any())
            //{
            //    dbMigrator.Update();
            //}
        }

        private static void EnsureLoadedForContext()
        {
            Assembly efAssembly = typeof(DbMigrator).Assembly;

            Type typeDbConfigurationManager =
                efAssembly.GetType(
                    "System.Data.Entity.Infrastructure.DependencyResolution.DbConfigurationManager"
                );

            PropertyInfo propertyInstance =
                typeDbConfigurationManager.GetProperty(
                    "Instance",
                    BindingFlags.Public | BindingFlags.Static
                );

            MethodInfo methodEnsureLoadedForContext =
                typeDbConfigurationManager.GetMethod(
                    nameof(EnsureLoadedForContext),
                    BindingFlags.Public | BindingFlags.Instance
                );

            object instance = propertyInstance.GetValue(null);

            methodEnsureLoadedForContext.Invoke(instance, new object[] { typeof(AZTDataContext) });
        }

        private static DbMigrator CreateDbMigrator(DbMigrationsConfiguration configuration, AZTDataContext context)
        {
            Type typeDbMigrator = typeof(DbMigrator);

            ConstructorInfo ctorInfo =
                typeDbMigrator
                    .GetConstructors(~BindingFlags.Public)
                    .FirstOrDefault(
                        ctor =>
                        {
                            ParameterInfo[] parameters = ctor.GetParameters();

                            if (parameters.Length != 2)
                            {
                                return false;
                            }

                            ParameterInfo param1 = parameters.First();
                            ParameterInfo param2 = parameters.Last();

                            bool isOK =
                                param1.Name == "configuration" && param1.ParameterType == typeof(DbMigrationsConfiguration)
                                &&
                                param2.Name == "usersContext" && param2.ParameterType == typeof(DbContext);

                            return isOK;
                        }
                    );

            object dbMigrator = ctorInfo.Invoke(new object[] { configuration, context });

            return dbMigrator as DbMigrator;
        }
    }

    internal sealed partial class Configuration : DbMigrationsConfiguration<AZTDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new SqlServerMigrationSqlGenerator());
        }
    }
}
