using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestProject.Models;
using TestProject.Tools;

namespace TestProject.Areas.Admin.Controllers
{
    
    [RoutePrefix("admin/api/Table")]
    public class TableController : ApiController
    {
        private UserDbContext db = new UserDbContext();
        
        public dynamic Get([FromUri] Filter filter)
        {
            var items = db.Users.AsQueryable();

            bool sortAsc = false;
            if (filter.SortOrder == "asc") sortAsc = true;

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
            if (!String.IsNullOrEmpty(filter.FilterStatus))
                items = items.Where(i => i.Roles.FirstOrDefault(p => p.Name.Equals(filter.FilterRoles)) != null);
            if (!String.IsNullOrEmpty(filter.FilterDateOfBirth))
            {
                string[] splitDate = filter.FilterDateOfBirth.Split('-');
                try
                {
                    DateTime begin = Convert.ToDateTime(splitDate[0]);
                    items = items.Where(i => i.DateOfBirth >= begin);

                    if (splitDate.Length == 2)
                    {
                        DateTime end = Convert.ToDateTime(splitDate[1]);
                        if (begin < end)
                            items = items.Where(i => i.DateOfBirth <= end);
                    }
                }
                catch (Exception ex) { }
                
            }

            if (!String.IsNullOrEmpty(filter.FilterAddedDate))
            {
                string[] splitDate = filter.FilterAddedDate.Split('-');
                try
                {
                    DateTime begin = Convert.ToDateTime(splitDate[0]);
                    items = items.Where(i => i.AddedDate >= begin);

                    if (splitDate.Length == 2)
                    {
                        DateTime end = Convert.ToDateTime(splitDate[1]);
                        if (begin < end)
                            items = items.Where(i => i.AddedDate <= end);
                    }
                }
                catch (Exception ex) { }
            }

            if (!String.IsNullOrEmpty(filter.FilterActivatedDate))
            {
                string[] splitDate = filter.FilterActivatedDate.Split('-');
                try
                {
                    DateTime begin = Convert.ToDateTime(splitDate[0]);
                    items = items.Where(i => i.ActivatedDate >= begin);

                    if (splitDate.Length == 2)
                    {
                        DateTime end = Convert.ToDateTime(splitDate[1]);
                        if (begin < end)
                            items = items.Where(i => i.ActivatedDate <= end);
                    }
                }
                catch (Exception ex) { }
            }

            if (!String.IsNullOrEmpty(filter.FilterLastVisitDate))
            {
                string[] splitDate = filter.FilterLastVisitDate.Split('-');
                try
                {
                    DateTime begin = Convert.ToDateTime(splitDate[0]);
                    items = items.Where(i => i.LastVisitDate >= begin);

                    if (splitDate.Length == 2)
                    {
                        DateTime end = Convert.ToDateTime(splitDate[1]);
                        if (begin < end)
                            items = items.Where(i => i.LastVisitDate <= end);
                    }
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
