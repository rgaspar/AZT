using Autofac;
using AutoMapper;
using Azure.TestProject.Common.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.AutoMapper
{
    public class AutoMapperDependencyModule : Module
    {
        private Type AutoMapperProfileBaseType { get; } = typeof(Profile);

        private IEnumerable<Type> GetAutoMapperProfileTypes()
        {
            ReadOnlyCollection<Type> types =
                AssembliesProvider
                    .GetAssemblies()
                    .SelectMany(assembly => assembly.ExportedTypes)
                    .Where(type => AutoMapperProfileBaseType.IsAssignableFrom(type))
                    .ToList()
                    .AsReadOnly();

            return types;
        }

        protected override void Load(ContainerBuilder builder)
        {
            IEnumerable<Type> profileTypes = GetAutoMapperProfileTypes();

            IMapper mapper =
                new MapperConfiguration(
                    config =>
                    {
                        config.AllowNullCollections = true;
                        config.AddMaps(profileTypes);
                    }
                )
                    .CreateMapper();

            builder.RegisterInstance(mapper);
        }
    }
}
