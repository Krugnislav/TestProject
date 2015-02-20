using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using TestProject.Models;
using TestProject.Tools;
using TestProject.Tools.Mail;

namespace TestProject.Areas.Admin.Controllers
{
    
//    [RoutePrefix("admin/api/Table")]
    public class TableController : ApiController
    {
        private UserDbContext db = new UserDbContext();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public dynamic Post([Bind(Include = "ID,Email,Password,Name,LastName,DateOfBirth")] User user)
        {
            return "Ok";
        }
        
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
            if (filter.FilterAddedDateEnd != null)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(filter.FilterAddedDateEnd.Trim(charsToTrim));
                    items = items.Where(i => i.AddedDate <= date);
                }
                catch (Exception ex) { }
            }

            if (filter.FilterActivatedDateStart != null)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(filter.FilterActivatedDateStart.Trim(charsToTrim));
                    items = items.Where(i => i.ActivatedDate >= date);
                }
                catch (Exception ex) { }
            }
            if (filter.FilterActivatedDateEnd != null)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(filter.FilterActivatedDateEnd.Trim(charsToTrim));
                    items = items.Where(i => i.ActivatedDate <= date);
                }
                catch (Exception ex) { }
            }

            if (filter.FilterLastVisitDateStart != null)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(filter.FilterLastVisitDateStart.Trim(charsToTrim));
                    items = items.Where(i => i.LastVisitDate >= date);
                }
                catch (Exception ex) { }
            }
            if (filter.FilterLastVisitDateEnd != null)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(filter.FilterLastVisitDateEnd.Trim(charsToTrim));
                    items = items.Where(i => i.LastVisitDate <= date);
                }
                catch (Exception ex) { }
            }

            int nTotalItems = items.Count();

            items = items.Skip((filter.PageNumber - 1) * filter.PageSize)
                         .Take(filter.PageSize);

            return new
            {
                totalItems = nTotalItems,
                items = items.ToArray()
            };
                
               
        }

    }
}
