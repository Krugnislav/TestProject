using System.Web.Http;
using System.Web.Mvc;

namespace TestProject.Areas.Default
{
    public class DefaultAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Default";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                name: "default_Api",
                routeTemplate: "api/{controller}"
            );

            context.Routes.MapHttpRoute(
                name: "default_ApiID",
                routeTemplate: "api/{controller}",
                defaults: null,
                    constraints: new { id = @"^\d+$" }
            );

            context.Routes.MapHttpRoute(
                name: "default_ApiWithAction",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            context.MapRoute(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TestProject.Areas.Default.Controllers" }
            );

        }
    }
}