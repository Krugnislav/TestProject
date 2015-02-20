using System.Web.Mvc;
using System.Web.Http;


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
            routeTemplate: "admin/api/{controller}/{id}",
            defaults: new { area = "admin", id = RouteParameter.Optional }
        );

            context.MapRoute(
                "Admin",
                "Admin/{controller}/{action}/{id}",
                new {controller = "LoginAdmin", action = "Index", id = UrlParameter.Optional }
            );
            
        
        }
    }
}