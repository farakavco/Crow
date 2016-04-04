using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Routing;

namespace slackk
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();

            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();
            //GlobalConfiguration.Configuration.Routes.MapHttpRoute(
            //    name: "Crow Api",
            //    routeTemplate: "api/{controller}/"
            //    );

            //config.Routes.MapHttpRoute(

            //);

            //config.Routes.MapHttpRoute("DefaultApiWithAction", "Api/{controller}/{action}");
        }
    }
}
