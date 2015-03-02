using Ninject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using TestProject.Global.Auth;
using TestProject.Global.Config;
using TestProject.Models;

namespace TestProject.Controllers
{
    public class BaseApiController : ApiController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static string HostName = string.Empty;

        [Inject]
        public IAuthentication Auth { get; set; }

        [Inject]
        public IConfig Config { get; set; }

        public User CurrentUser
        {
            get
            {
      
                User user = ((IUserProvider)Auth.CurrentUser.Identity).User;
                return user;
            }
        }

        protected override void Initialize(HttpControllerContext requestContext)
        {
            if (HttpContext.Current.Request.Url != null)
            {
                HostName = HttpContext.Current.Request.Url.Authority;
            }
            try
            {
                var cultureInfo = new CultureInfo(Config.Lang);

                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
            catch (Exception ex)
            {
                logger.Error("Culture not found", ex);
            }

            base.Initialize(requestContext);
        }
    }
}
