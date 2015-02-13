using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProject.Controllers;
using TestProject.Tools.Mail;

namespace TestProject.Areas.Default.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserLogin()
        {
            var user = CurrentUser;
            
            return PartialView(CurrentUser);
        }

        public ActionResult SendEmail()
        {
            NotifyMail.SendNotify("Register", "stanislav.kruglov@simbirsoft.com",
                subject => string.Format(subject, HostName),
                body => string.Format(body, "", HostName));

            return RedirectToAction("Index", "Home");
        }

    }
}