using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjConcept.CustomAuth
{
    public class BasicAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        AuthorizeUser auth = new AuthorizeUser();

        // Not overriding AuthorizeCore because it only has access to the HttpContext which does not contain the information required.
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.AuthorizeCore(filterContext.HttpContext);
            //base.OnAuthorization(filterContext);

            // Custom authorization
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string action = filterContext.ActionDescriptor.ActionName;
            if (!(controller == "Home" && action == "Unauthorized"))
            {
                bool userAuthorized = auth.AuthorizeSecurityLevel(filterContext);
                if (!userAuthorized)
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Unauthorized" }));
            }
        }
    }
}