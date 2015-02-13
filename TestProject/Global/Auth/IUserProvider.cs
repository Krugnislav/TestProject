using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestProject.Models;

namespace TestProject.Global.Auth
{
    public interface IUserProvider
    {
        User User { get; set; }
    }
}