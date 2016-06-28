using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;
using JwtWithWebAPI.IoCConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JwtWithWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = SmObjectFactory.Container;
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator), new SmWebApiControllerActivator(container));

            config.Services.Replace(typeof(IFilterProvider), new SmWebApiFilterProvider(container));

            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            setSerializerSettings();
        }

        private static void setSerializerSettings()
        {
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}