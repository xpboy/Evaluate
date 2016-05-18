using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evaluate.Controllers
{
    public class NavigationController : Controller
    {
        [ChildActionOnly]
        public ActionResult Menu()
        {
            if (User.IsInRole("Admins")) // TODO: Remove magic string
            {
                return PartialView("_AdminMenu");
            }
            return new EmptyResult();
        }
    }
}