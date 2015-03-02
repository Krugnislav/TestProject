using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using TestProject.Controllers;
using TestProject.Models;
using TestProject.Models.ViewModels;
using TestProject.Tools.Mail;

namespace TestProject.Areas.Default.Controllers
{
    public class UserController : BaseController
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private UserDbContext db = new UserDbContext();
        // GET: Default/User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Activate(string id)
        {
                if (id != null)
            {
                var user = db.Activated(id);
                if ((user != null) && (user.ActivatedDate == null))
                {
                    user.ActivatedDate = DateTime.Now;
                    user.Status = "Активирован";
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    // CurrentUser = user;
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public bool CheckAnEmail(string email)
        {
            bool result = db.Users.Where(i => i.Email.Equals(email)).Count() == 0;
            return result;
        }

        public ActionResult Create()
        {
            var userAuth = CurrentUser;
            if (userAuth != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public ActionResult GoodCreate()
        {
            return View();
        }

        public ActionResult ShortMenu()
        {
            var user = CurrentUser;

            return PartialView(CurrentUser);
        }

        public ActionResult PrivateOffice()
        {
            var userAuth = CurrentUser;
            if (userAuth == null)
                return RedirectToAction("Index", "Home");
            if (userAuth.AvatarPath == null) userAuth.AvatarPath = "~/Content/file/default-avatar.png";
            return View(SerializeObject(userAuth));
        }

        public static IHtmlString SerializeObject(object value)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                var serializer = new JsonSerializer
                {
                    // Let's use camelCasing as is common practice in JavaScript
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                // We don't want quotes around object names
                jsonWriter.QuoteName = false;
                serializer.Serialize(jsonWriter, value);

                return new HtmlString(stringWriter.ToString());
            }
        }

        public ActionResult EditAvatar()
        {
            return PartialView();
        }

        public ActionResult ChangePassword()
        {
            var userAuth = CurrentUser;
            if (userAuth != null)
                return RedirectToAction("Index", "Home");
            return View(new LoginView());
        }

        [HttpPost]
        public ActionResult ChangePassword(LoginView loginView)
        {
            User user = db.Users.FirstOrDefault(p => p.Email.Equals(loginView.Email));
            if (user != null)
                NotifyMail.SendNotify("Change", user.Email,
                        subject => string.Format(subject, HostName),
                        body => string.Format(body, HttpUtility.UrlEncode(user.ActivatedLink), HostName));

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePass(string id)
        {
            var userAuth = CurrentUser;
            if (userAuth == null)
                if (id != null)
                {
                    var user = db.Activated(id);
                    if (user != null)
                    {
                        // CurrentUser = user;
                        return View(user);
                    }
                }
            return RedirectToAction("Index", "Home");
        }

    }
}