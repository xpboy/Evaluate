using Evaluate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evaluate.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Subscriptions()
        {
            return View(db.Plans.ToList());
        }

        public ActionResult Links()
        {
            return View(db.Links.OrderBy(o => o.LinkName).ToList());
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Contact()
        //{
        //    if (ModelState.IsValid)
        //    {

        //    }
        //    return View();
        //}
    }
}