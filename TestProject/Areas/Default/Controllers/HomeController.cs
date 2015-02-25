using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProject.Controllers;
using TestProject.Models;
using TestProject.Tools.Mail;

namespace TestProject.Areas.Default.Controllers
{
    public class HomeController : BaseController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private UserDbContext db = new UserDbContext();

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

        public ActionResult Create()
        {
            var userAuth = CurrentUser;
            if (userAuth != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Home/Register
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Email,Password,Name,LastName,DateOfBirth")] User user)
        {

            logger.Info("User try to registr");
            if (ModelState.IsValid)
            {
                logger.Info("User registrated");
                user.AddedDate = System.DateTime.Now;
                user.ActivatedLink = user.GetActivateUrl();
                NotifyMail.SendNotify("Register", user.Email,
                    subject => string.Format(subject, HostName),
                    body => string.Format(body, HttpUtility.UrlEncode(user.ActivatedLink), HostName));
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            logger.Warn("Couldn't registrate");

            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

    }
}