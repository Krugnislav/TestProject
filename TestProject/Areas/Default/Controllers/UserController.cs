using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProject.Controllers;
using TestProject.Models;

namespace TestProject.Areas.Default.Controllers
{
    public class UserController : BaseController
    {
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
                if (user != null)
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

    }
}