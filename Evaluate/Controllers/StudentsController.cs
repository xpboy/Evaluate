using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Evaluate.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Evaluate.ViewModels;
using Microsoft.AspNet.Identity;

namespace Evaluate.Controllers
{
    //[Authorize(Roles="Admin")]
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var users = GetSuscriptions(null);

            return View(users);

            //DateTime today = DateTime.Now;
            //DateTime past = DateTime.Now.AddDays(-30);
            //var users = (from u in db.Users
            //             join s in db.Students on u.Id equals s.UserId
            //             join sc in db.StudentCareers on s.StudentId equals sc.StudentId
            //             where u.EmailConfirmed == true
            //             && s.Status == true
            //             //&& sc.StatusId != (int)_SubscriptionStatus.Canceled
            //             //&& today >= sc.StartDate
            //             //&& today <= sc.EndDate
            //             && sc.CreatedDate >= past
            //             select new SubscriptionsViewModel
            //              {
            //                  UserId = u.Id,
            //                  Email = u.Email,
            //                  Name = u.Name,
            //                  LastName = u.LastName,
            //                  CareerName = sc.Career.CareerName,
            //                  StartDate = sc.StartDate,
            //                  EndDate = sc.EndDate,
            //                  StudentCareerId = sc.Id,
            //                  StatusId = sc.StatusId
            //              }).OrderBy(o => o.StartDate);

            //return View(users.ToList());

            //var users = db.Users.Where(u => u.EmailConfirmed == true
            //    && u.Roles.Select(r => r.RoleId).Contains("2"));
            //return View(users.ToList());

            //var users = db.Users
            //    .Where(x => x.Roles
            //        .Any(y => y.RoleId.Equals(db.Roles
            //            .Where(a => a.Name.Equals("Employees"))
            //            .Select(b => b.Id).FirstOrDefault()))).ToList();

            //var students = db.Students;
            //return View(students.ToList());
        }

        [Authorize(Roles = "Student")]
        [SessionExpire(SessionName = "student")]
        public ActionResult Suscriptions()
        {
            //var users = GetSuscriptions(((Student)Session["student"]).StudentId);

            //return View(users);
            return View();
        }

        [Authorize(Roles = "Student")]
        [ChildActionOnly]
        [SessionExpire(SessionName = "student")]
        public ActionResult StudentSuscriptions()
        {
            var users = GetSuscriptions(((Student)Session["student"]).StudentId);
            return PartialView("_StudentSuscriptions", users);
        }

        public List<SubscriptionsViewModel> GetSuscriptions(int? studentId)
        {
            DateTime today = DateTime.Now;
            DateTime past = DateTime.Now.AddDays(-30);
            var users = (from u in db.Users
                         join s in db.Students on u.Id equals s.UserId
                         join sc in db.StudentCareers on s.StudentId equals sc.StudentId
                         where u.EmailConfirmed == true
                         && (s.StudentId == studentId || studentId == null)
                         && s.Status == true
                             //&& sc.StatusId != (int)_SubscriptionStatus.Canceled
                             //&& today >= sc.StartDate
                             //&& today <= sc.EndDate
                         && sc.CreatedDate >= past
                         select new SubscriptionsViewModel
                         {
                             UserId = u.Id,
                             Email = u.Email,
                             Name = u.Name,
                             LastName = u.LastName,
                             CareerName = sc.Career.CareerName,
                             StartDate = sc.StartDate,
                             EndDate = sc.EndDate,
                             StudentCareerId = sc.Id,
                             StatusId = sc.StatusId
                         }).OrderBy(o => o.StartDate);

            return users.ToList();
        }

        // GET: Students/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.PersonId = new SelectList(db.People, "PersonId", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,PersonId,Status,CreatedBy,CreatedDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PersonId = new SelectList(db.People, "PersonId", "Name", student.UserId);
            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonId = new SelectList(db.People, "PersonId", "Name", student.UserId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,PersonId,Status,CreatedBy,CreatedDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                //ApplicationUser user = db.Users.Where(u => u.PersonId == student.PersonId).FirstOrDefault();

                AddRoleToUser(User.Identity.Name, "Student");
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonId = new SelectList(db.People, "PersonId", "Name", student.UserId);
            return View(student);
        }

        public void AddRoleToUser(string UserName, string RoleName)
        {
            ApplicationUser user = db.Users
                .Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var account = new AccountController();
            //account.UserManager.AddToRole(user.Id, RoleName);

            var role = db.Roles.SingleOrDefault(m => m.Name == RoleName);
            user.Roles.Add(new IdentityUserRole { RoleId = role.Id });
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
