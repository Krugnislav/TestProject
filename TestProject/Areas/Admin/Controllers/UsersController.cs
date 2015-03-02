using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TestProject.Controllers;
using TestProject.Models;
using TestProject.Tools.Mail;

namespace TestProject.Areas.Admin.Controllers
{
    [Route("admin/api/Users")]
    public class UsersController : BaseController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private UserDbContext db = new UserDbContext();

        private string RandomString(int size, Random random)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private void FillTable() {
            Random rand = new Random();
            DateTime start = new DateTime(1945, 1, 1);
            int range = (DateTime.Today - start).Days;
            for (int i = 0; i < 100000; i++)
            {
                int status = rand.Next(2);
                User user = new User();
                string EmailP1 = RandomString(10, rand);
                string EmailP2 = RandomString(4, rand);
                string Email = EmailP1 + "@" + EmailP2 + ".com";
                while (!CheckAnEmail(Email)) 
                {
                    EmailP1 = RandomString(10, rand);
                    EmailP2 = RandomString(4, rand);
                    Email = EmailP1 + "@" + EmailP2 + ".com";
                } 
                user.Email = Email;
                user.Name = RandomString(10, rand);
                user.LastName = RandomString(10, rand);
                user.Password = RandomString(10, rand);
                user.AddedDate = System.DateTime.Now;
                user.ActivatedLink = user.GetActivateUrl();
                user.ActivatedDate = System.DateTime.Now;
                if (status == 1) user.Status = "Активирован";
                else user.Status = "Дезактивирован";
                user.DateOfBirth = start.AddDays(rand.Next(range));
                db.Users.Add(user);
                db.SaveChanges();
            }
        }



        public ActionResult UserLogin()
        {
            var user = CurrentUser;

            return PartialView(CurrentUser);
        }

        public ActionResult EditRow()
        {
            return PartialView();
        }

        // GET: Users
        public ActionResult Index()
        {
            //FillTable();

            var user = CurrentUser;
            if (user != null)
                if (user.Roles.FirstOrDefault(p => p.ID == 1) != null)
                    return View();
            return RedirectToAction("Index", "LoginAdmin");
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var user = CurrentUser;
            if ((user == null) || (user.Roles.FirstOrDefault(p => p.ID == 1) == null))
                return RedirectToAction("Index", "LoginAdmin");
            return PartialView();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Email,Password,Name,LastName,DateOfBirth")] User user)
        {
            var userAuth = CurrentUser;
            if ((userAuth == null) || (userAuth.Roles.FirstOrDefault(p => p.ID == 1) == null))
                return RedirectToAction("Index", "LoginAdmin");
            
            logger.Info("Try to create user");
            if ((ModelState.IsValid) && (CheckAnEmail(user.Email)))
            {
                logger.Info("User created");
                user.AddedDate = System.DateTime.Now;
                user.ActivatedLink = user.GetActivateUrl();
                NotifyMail.SendNotify("Register", user.Email,
                    subject => string.Format(subject, HostName),
                    body => string.Format(body, HttpUtility.UrlEncode(user.ActivatedLink), HostName));
                db.Users.Add(user);
                db.SaveChanges();
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }

            logger.Warn("Couldn't create");

            return Json("Invalid form!", JsonRequestBehavior.AllowGet);
        }


        // GET: Users/Edit/5
        public ActionResult Edit()
        {
            return PartialView();
        }
    
        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Email,Password,Name,LastName,DateOfBirth,AddedDate,ActivatedDate,ActivatedLink,LastVisitDate,AvatarPath,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool CheckAnEmail(string email)
        {
            bool result = db.Users.Where(i => i.Email.Equals(email)).Count() == 0;
            return result;
        }

    }
}
