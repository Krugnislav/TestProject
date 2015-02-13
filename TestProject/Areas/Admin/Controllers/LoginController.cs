using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProject.Controllers;
using TestProject.Models.ViewModels;

namespace TestProject.Areas.Admin.Controllers
{
    public class LoginAdminController : BaseController
    {
        public ActionResult Index()
        {
            
            return View(new LoginView());
        }

        [HttpPost]
        public ActionResult Index(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                var user = Auth.Login(loginView.Email, loginView.Password, loginView.IsPersistent);
                if (user != null)
                {
                    if (user.Roles.FirstOrDefault(p => p.ID == 1) != null)
                        return RedirectToAction("Index", "Users");
                    ModelState["Email"].Errors.Add("Нет прав доступа");
                }
                ModelState["Password"].Errors.Add("Пароли не совпадают");
            }
            return View(loginView);
        }

        public ActionResult Logout()
        {
            Auth.LogOut();
            return RedirectToAction("Index", "Users");
        }
    }

    
}