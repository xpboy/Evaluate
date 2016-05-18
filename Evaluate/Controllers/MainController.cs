using Evaluate.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Evaluate.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Main/Index
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard");
            }
            else if (User.IsInRole("Student"))
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var user = UserManager.FindByName(User.Identity.Name);
                if (user.EmailConfirmed == true)
                {
                    LoadStudent(user.Id);
                    return View();
                }
                else
                {
                    LoadStudent(user.Id);
                    return RedirectToAction("Waiting");
                }
            }
            else
            {
                //LoadStudent();
                return RedirectToAction("Waiting");
            }
        }

        public void LoadStudent(string userId)
        {
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            //var user = UserManager.FindByName(User.Identity.Name);

            Student student = db.Students.Where(s => s.UserId == userId).FirstOrDefault();

            Session["student"] = student;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Waiting()
        {
            return View();
        }        
    }
}