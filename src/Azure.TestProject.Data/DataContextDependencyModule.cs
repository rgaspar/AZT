using Autofac;
using Azure.TestProject.Common.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Data
{
    public sealed class DataContextDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AZTDataContext>().AsSelf().InstancePerApplicationKind();
        }
    }
}
