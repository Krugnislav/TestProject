﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TestProject.Controllers;
using TestProject.Models;
using TestProject.Tools.Mail;

namespace TestProject.Areas.Default.Controllers
{

    public class UserEditController : BaseApiController
    {
        private UserDbContext db = new UserDbContext();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [System.Web.Http.ActionName("Create")]
        public HttpResponseMessage PostCreate([Bind(Include = "ID,Email,Password,Name,LastName,DateOfBirth")] User user)
        {
            logger.Info("User try to registr");
            if ((ModelState.IsValid) && (CheckAnEmail(user.Email)))
            {
                logger.Info("User registrated");
                user.AddedDate = System.DateTime.Now;
                user.ActivatedLink = user.GetActivateUrl();
                NotifyMail.SendNotify("Register", user.Email,
                    subject => string.Format(subject, HostName),
                    body => string.Format(body, HttpUtility.UrlEncode(user.ActivatedLink), HostName));
                db.Users.Add(user);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            logger.Warn("Couldn't registrate");
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [System.Web.Http.ActionName("Edit")]
        public HttpResponseMessage PostEdit([Bind(Include = "ID,Name,LastName,DateOfBirth,AvatarPath")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [System.Web.Http.ActionName("CheckAnEmail")]
        public HttpResponseMessage Get(string email)
        {
            if (db.Users.Where(i => i.Email.Equals(email)).Count() == 0)
                return Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        public bool CheckAnEmail(string email)
        {
            bool result = db.Users.Where(i => i.Email.Equals(email)).Count() == 0;
            return result;
        }

    }
}