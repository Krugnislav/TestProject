using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestProject.Models;

namespace TestProject.Areas.Admin.Controllers
{
    [RoutePrefix("admin/api/Table")]
    public class TableController : ApiController
    {
        private UserDbContext db = new UserDbContext();
        
        [HttpGet]
        public dynamic Get()
        {
            var items = db.Users.AsQueryable();

            int nTotalItems = items.Count();

            return new
            {
                totalItems = nTotalItems,
                items = items.ToArray()
            };
        }

    }
}
