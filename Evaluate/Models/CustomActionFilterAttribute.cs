using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Evaluate.Models
{
    public class SessionExpire : ActionFilterAttribute
    {
        public string SessionName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var redirectTarget = new RouteValueDictionary { { "action", "index" }, { "controller", "main" } };

            if (session["" + SessionName + ""] != null)
                return;

            //Redirect him to somewhere.
            redirectTarget = new RouteValueDictionary { 
                    { "action", "index" }, 
                    { "controller", "main" } 
                };
            filterContext.Result = new RedirectToRouteResult(redirectTarget);
        }
    }

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    //public class NewAuthoriseAttribute : ActionFilterAttribute
    //{
    //    public string IdParamName { get; set; }

    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        if (filterContext.ActionParameters.ContainsKey(IdParamName))
    //        {
    //            var id = filterContext.ActionParameters[IdParamName] as Int32?;
    //        }
    //    }
    //}

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    //public class SessionExpireFilterAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        HttpContext ctx = HttpContext.Current;

    //        // If the browser session or authentication session has expired...
    //        if (ctx.Session["UserName"] == null || !filterContext.HttpContext.Request.IsAuthenticated)
    //        {
    //            if (filterContext.HttpContext.Request.IsAjaxRequest())
    //            {
    //                // For AJAX requests, we're overriding the returned JSON result with a simple string,
    //                // indicating to the calling JavaScript code that a redirect should be performed.
    //                filterContext.Result = new JsonResult { Data = "_Logon_" };
    //            }
    //            else
    //            {
    //                // For round-trip posts, we're forcing a redirect to Home/TimeoutRedirect/, which
    //                // simply displays a temporary 5 second notification that they have timed out, and
    //                // will, in turn, redirect to the logon page.
    //                filterContext.Result = new RedirectToRouteResult(
    //                    new RouteValueDictionary {
    //                    { "Controller", "Home" },
    //                    { "Action", "TimeoutRedirect" }
    //            });
    //            }
    //        }

    //        base.OnActionExecuting(filterContext);
    //    }
    //}
}