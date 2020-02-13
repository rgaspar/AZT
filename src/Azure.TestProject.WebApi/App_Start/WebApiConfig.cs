using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Azure.TestProject.WebApi
{
    public static partial class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.ConfigureAutofac();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
