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
        public string FilterDateOfBirthStart { get; set; }
        public string FilterDateOfBirthEnd { get; set; }
        public string FilterAddedDateStart { get; set; }
        public string FilterAddedDateEnd { get; set; }
        public string FilterActivatedDateStart { get; set; }
        public string FilterActivatedDateEnd { get; set; }
        public string FilterLastVisitDateStart { get; set; }
        public string FilterLastVisitDateEnd { get; set; }
        public string FilterStatus { get; set; }
        public string FilterRoles { get; set; }
        
    }
}