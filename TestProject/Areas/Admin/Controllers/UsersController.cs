using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestProject.Controllers;
using TestProject.Models;
using TestProject.Tools.Mail;

namespace TestProject.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private UserDbContext db = new UserDbContext();

        public ActionResult UserLogin()
        {
            var user = CurrentUser;

            return PartialView(CurrentUser);
        }

        // GET: Users
        public ActionResult Index()
        {
            var user = CurrentUser;
            if (user != null)
                if (user.Roles.FirstOrDefault(p => p.ID == 1) != null)
                return View(db.Users.ToList());
            return RedirectToAction("Index", "LoginAdmin");
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Email,Password,Name,LastName,DateOfBirth")] User user)
        {
            logger.Info("Try to create user");
            if (ModelState.IsValid)
            {
                logger.Info("User created");
                user.AddedDate = System.DateTime.Now;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            logger.Warn("Couldn't create");

            return View(user);
        }


        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
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
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult CheckEmail(string email)
        {
            var result = db.Users.Where(i => i.Email.Equals(email)).Count() == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
