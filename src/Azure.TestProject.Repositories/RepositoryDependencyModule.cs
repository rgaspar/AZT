using Autofac;
using Azure.TestProject.Common.DependencyInjection;
using Azure.TestProject.Data;
using Azure.TestProject.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Repositories
{
    public class RepositoryDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterUnitsOfWork(builder);
            RegisterCoreServices(builder);
        }

        private void RegisterUnitsOfWork(ContainerBuilder builder)
        {
            builder
                .RegisterType<UnitOfWork<AZTDataContext>>()
                .Named<IUnitOfWork>("Default")
                .InstancePerApplicationKind();
        }

        private void RegisterCoreServices(ContainerBuilder builder)
        {
            builder
                .RegisterType<EmailAttribureRepository>()
                .As<IEmailAttribureRepository>()
                .InstancePerApplicationKind();
        }
    }
}
