using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProject.Controllers;
using TestProject.Models;
using TestProject.Models.ViewModels;
using TestProject.Tools.Mail;

namespace TestProject.Areas.Default.Controllers
{
    public class HomeController : BaseController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult UserLogin()
        {
            var user = CurrentUser;
            
            return PartialView(CurrentUser);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return PartialView(new LoginView());
        }

    }
}