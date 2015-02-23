using System.Web.Mvc;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Net.Http;


namespace TestProject.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.Routes.MapHttpRoute(
                name: "Admin_Api",
                routeTemplate: "admin/api/{controller}"
            );

            context.Routes.MapHttpRoute(
                name: "Admin_ApiID",
                routeTemplate: "admin/api/{controller}", 
                defaults: null,
                    constraints: new { id = @"^\d+$" }
            );

            context.Routes.MapHttpRoute(
                name: "Admin_ApiWithAction",
                routeTemplate: "admin/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            

            context.MapRoute(
                "Admin",
                "Admin/{controller}/{action}/{id}",
                new {controller = "LoginAdmin", action = "Index", id = UrlParameter.Optional }
            );
            
        
        }
    }
}