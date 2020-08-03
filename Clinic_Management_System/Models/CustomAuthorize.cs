using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic_Management_System.Models
{
    public class CustomAuthorize:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (this.AuthorizeCore(filterContext.HttpContext))
            { base.OnAuthorization(filterContext); }

            else if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            { filterContext.Result = new RedirectResult("~/Home/Login"); }
            else
            { filterContext.Result = new RedirectResult("~/CustomError/Unauthorized"); }
        }
    }
}