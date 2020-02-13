using Autofac;
using Azure.TestProject.Common.DependencyInjection;
using Azure.TestProject.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Services
{
    public class ServiceDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterCoreServices(builder);
        }

        private void RegisterCoreServices(ContainerBuilder builder)
        {
            builder
                .RegisterType<EmailAttributesService>()
                .As<IEmailAttributesService>()
                .InstancePerApplicationKind();
        }
    }
}
