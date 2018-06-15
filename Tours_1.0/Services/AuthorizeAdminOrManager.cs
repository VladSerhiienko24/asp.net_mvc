using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tours_1._0.Controllers
{
    public class AuthorizeAdminOrManager: AuthorizeAttribute
    {
        public AuthorizeAdminOrManager()
        {
            Roles = "admin, manager";
        }
    }

    public class AuthorizeManagerOrUser : AuthorizeAttribute
    {
        public AuthorizeManagerOrUser()
        {
            Roles = "manager, user";
        }
    }

    public class AuthorizeAdminOrUser : AuthorizeAttribute
    {
        public AuthorizeAdminOrUser()
        {
            Roles = "admin, user";
        }
    }
}