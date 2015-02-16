using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TestProject
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
            name: "API Area Default",
            routeTemplate: "api/Areas/Admin/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
        }
    }
}
