using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using Azure.TestProject.AutoMapper;
using Azure.TestProject.Common.DependencyInjection;
using Azure.TestProject.Common.Enumerations;
using Azure.TestProject.Data;
using Azure.TestProject.Repositories;
using Azure.TestProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace Azure.TestProject.WebApi
{
    public static partial class WebApiConfig
    {
        public static void ConfigureAutofac(this HttpConfiguration httpConfiguration)
        {
            // TODO : later implement plugin module...
            Assembly[] aztAssemblies = AssembliesProvider.GetAssemblies();

            // Hack... plugin mode solve this problem
            var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            var assembliesTypes = assemblyNames
                .Where(a => a.FullName.Contains("Azure.TestProject"))
                .SelectMany(an => Assembly.Load(an).GetTypes())
                .Where(p => typeof(Profile).IsAssignableFrom(p) && p.IsPublic && !p.IsAbstract)
                .Distinct();

            var autoMapperProfiles = assembliesTypes.Select(p => (Profile)Activator.CreateInstance(p)).ToList();

            ApplicationKindProvider.Initialize(ApplicationKind.AspNetWebApi2);

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(httpConfiguration);
            //builder.RegisterAssemblyModules(aztAssemblies);

            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                foreach (var profile in autoMapperProfiles)
                {
                    cfg.AddProfile(profile);
                }
            }));
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().InstancePerLifetimeScope();


            builder.RegisterModule<RepositoryDependencyModule>();
            builder.RegisterModule<ServiceDependencyModule>();
            //builder.RegisterModule<AutoMapperDependencyModule>();
            builder.RegisterModule<DataContextDependencyModule>();


            IContainer container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

    }
}