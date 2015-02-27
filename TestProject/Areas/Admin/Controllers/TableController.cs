using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using TestProject.Controllers;
using TestProject.Models;
using TestProject.Tools;
using TestProject.Tools.Mail;

namespace TestProject.Areas.Admin.Controllers
{
    
    public class TableController : BaseApiController
    {
        private UserDbContext db = new UserDbContext();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [System.Web.Http.ActionName("Users")]
        public HttpResponseMessage Post([Bind(Include = "ID,Name,LastName,DateOfBirth,AvatarPath,Status, Roles")] User user)
        {
            if (ModelState.IsValid)
            {
                User userEdited = db.Users.Find(user.ID);
                if (userEdited.Status != user.Status)
                {
                    NotifyMail.SendNotify("Edit", user.Email,
                        subject => string.Format(subject, HostName),
                        body => string.Format(body, user.Status, HostName));
                }
                userEdited.Name = user.Name;
                userEdited.LastName = user.LastName;
                userEdited.DateOfBirth = user.DateOfBirth;
                userEdited.Status = user.Status;

                userEdited.Roles.Clear();
                foreach (var role in user.Roles)
                {
                    userEdited.Roles.Add(db.Roles.Find(role.ID));
                }

                db.Entry(userEdited).State = EntityState.Modified;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
        
        [System.Web.Http.ActionName("Roles")]
        public IQueryable<Role> GetAllRoles()
        {
            return db.Roles.AsQueryable();
        }

        [System.Web.Http.ActionName("Users")]
        public dynamic Get([FromUri] TestProject.Models.Filter filter)
        {
            var items = db.Users.AsQueryable();

            bool sortAsc = false;
            if (filter.SortOrder == "asc") sortAsc = true;
            char[] charsToTrim = { '"', '\'' };

            items = items.OrderByFieldName(filter.SortColumn, sortAsc);

            items = items.Where(i => i.ActivatedDate != null);

            if (!String.IsNullOrEmpty(filter.FilterEmail))
                items = items.Where(i => i.Email.ToLower().Contains(filter.FilterEmail.ToLower()));
            if (!String.IsNullOrEmpty(filter.FilterName))
                items = items.Where(i => i.Name.ToLower().Contains(filter.FilterName.ToLower()));
            if (!String.IsNullOrEmpty(filter.FilterLastName))
                items = items.Where(i => i.LastName.ToLower().Contains(filter.FilterLastName.ToLower()));
            if (!String.IsNullOrEmpty(filter.FilterStatus))
                items = items.Where(i => i.Status.Equals(filter.FilterStatus.ToLower()));
            if (!String.IsNullOrEmpty(filter.FilterRoles))
                items = items.Where(i => i.Roles.FirstOrDefault(p => p.Name.Equals(filter.FilterRoles)) != null);
            if (filter.FilterDateOfBirthStart != null)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(filter.FilterDateOfBirthStart.Trim(charsToTrim));
                    items = items.Where(i => i.DateOfBirth >= date);
                }
                catch (Exception ex) { }
            }
            if (filter.FilterDateOfBirthEnd != null)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(filter.FilterDateOfBirthEnd.Trim(charsToTrim));
                    items = items.Where(i => i.DateOfBirth <= date);
                }
                catch (Exception ex) { }
            }

            if (filter.FilterAddedDateStart != null)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(filter.FilterAddedDateStart.Trim(charsToTrim));
                    items = items.Where(i => i.AddedDate >= date);
                }
                catch (Exception ex) { }
            }

            int nTotalItems = items.Count();

            items = items.Skip((filter.PageNumber - 1) * filter.PageSize)
                         .Take(filter.PageSize);

            return new
            {
                roles = db.Roles.AsQueryable(),
                totalItems = nTotalItems,
                items = items.ToArray()
            };
                
               
        }

    }
}
