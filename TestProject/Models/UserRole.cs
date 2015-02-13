using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProject.Models
{
    public class UserRole
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int RoleID { get; set; }
    }
}