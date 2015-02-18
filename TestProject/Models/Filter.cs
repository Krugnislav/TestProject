using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProject.Models
{
    public class Filter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }

        public string FilterName { get; set; }
        public string FilterID { get; set; }
        public string FilterEmail { get; set; }
        public string FilterLastName { get; set; }
        public string FilterDateOfBirth { get; set; }
        public string FilterAddedDate { get; set; }
        public string FilterActivatedDate { get; set; }
        public string FilterLastVisitDate { get; set; }
        public string FilterStatus { get; set; }
        public string FilterRoles { get; set; }
        
    }
}