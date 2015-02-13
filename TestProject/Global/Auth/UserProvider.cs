using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using TestProject.Models;

namespace TestProject.Global.Auth
{
    public class UserProvider : IPrincipal
    {
        private UserIndentity userIdentity { get; set; }

        #region IPrincipal Members

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public IIdentity Identity
        {
            get
            {
                return userIdentity;
            }
        }

        /// <summary>
        /// Находится в данной роли или нет
        /// </summary>
        /// <param name="role">имя роли</param>
        /// <returns>есть такая роль или нет</returns>
        public bool IsInRole(string role)
        {
            if (userIdentity.User == null)
            {
                return false;
            }
            return userIdentity.User.Roles.FirstOrDefault(p => p.Code.Equals(role)) != null;
        }

        #endregion

        /// <summary>
        /// конструктор  
        /// </summary>
        /// <param name="name"></param>
        /// <param name="repository"></param>
        public UserProvider(string name, UserDbContext db)
        {
            userIdentity = new UserIndentity();
            userIdentity.Init(name, db);
        }


        /// <summary>
        /// Имя пользователя
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return userIdentity.Name;
        }
    }
}