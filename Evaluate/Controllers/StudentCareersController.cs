using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Evaluate.Models;

namespace Evaluate.Controllers
{
    [Authorize]
    public class StudentCareersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StudentCareers
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            DateTime today = DateTime.Now;
            DateTime past = DateTime.Now.AddDays(-30);
            var users = (from u in db.Users
                         join s in db.Students on u.Id equals s.UserId
                         join sc in db.StudentCareers on s.StudentId equals sc.StudentId
                         where u.EmailConfirmed == true
                         && s.Status == true
                         && sc.StatusId == (int)_SubscriptionStatus.Requested
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
                             StatusId = sc.StatusId,
                             PlanName = sc.Plan.PlanName
                         }).OrderBy(o => o.StartDate);

            return View(users.ToList());

            //var studentCareers = db.StudentCareers.Include(s => s.Career).Include(s => s.Plan).Include(s => s.Student);
            //return View(studentCareers.ToList());
        }

        // GET: StudentCareers/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCareer studentCareer = db.StudentCareers.Find(id);
            if (studentCareer == null)
            {
                return HttpNotFound();
            }
            return View(studentCareer);
        }

        // GET: StudentCareers/Create
        //[Authorize(Roles="Student")]
        [SessionExpire(SessionName = "student")]
        public ActionResult Create()
        {
            ViewBag.CareerId = new SelectList(db.Careers.Where(c => c.Status == true), "CareerId", "CareerName");
            ViewBag.PlanId = new SelectList(db.Plans.Where(p => p.Status == true), "PlanId", "PlanName");
            //ViewBag.StudentId = new SelectList(db.Students, "StudentId", "UserId");
            return View();
        }

        // POST: StudentCareers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Student")]
        [SessionExpire(SessionName = "student")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,CareerId,CreatedBy,CreatedDate,Status,PlanId,Months,StartDate,EndDate,Price")] StudentCareer studentCareer)
        {
            if (ModelState.IsValid)
            {
                var plan = db.Plans.Find(studentCareer.PlanId);
                studentCareer.StudentId = ((Student)Session["student"]).StudentId;
                studentCareer.CreatedBy = 1;
                studentCareer.CreatedDate = DateTime.Now;
                studentCareer.StatusId = (int)_SubscriptionStatus.Requested;
                //studentCareer.Months = plan.Months;
                studentCareer.Interval = plan.Interval;
                studentCareer.Increment = plan.Increment;
                //
                //
                studentCareer.Price = plan.Price;
                db.StudentCareers.Add(studentCareer);
                db.SaveChanges();
                return RedirectToAction("Index", "Main");
            }

            ViewBag.CareerId = new SelectList(db.Careers.Where(c => c.Status == true), "CareerId", "CareerName", studentCareer.CareerId);
            ViewBag.PlanId = new SelectList(db.Plans.Where(p => p.Status == true), "PlanId", "PlanName", studentCareer.PlanId);
            //ViewBag.StudentId = new SelectList(db.Students, "StudentId", "UserId", studentCareer.StudentId);
            return View(studentCareer);
        }

        // GET: StudentCareers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCareer studentCareer = db.StudentCareers.Find(id);
            if (studentCareer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CareerId = new SelectList(db.Careers.Where(c => c.Status == true), "CareerId", "CareerName", studentCareer.CareerId);
            ViewBag.PlanId = new SelectList(db.Plans.Where(p => p.Status == true), "PlanId", "PlanName", studentCareer.PlanId);
            //ViewBag.StudentId = new SelectList(db.Students, "StudentId", "UserId", studentCareer.StudentId);
            return View(studentCareer);
        }

        // POST: StudentCareers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,CareerId,CreatedBy,CreatedDate,Status,PlanId,Months,StartDate,EndDate,Price")] StudentCareer studentCareer, string Action)
        {
            if (ModelState.IsValid)
            {
                if (Action == "Autorizar")
                {
                    var plan = db.Plans.Find(studentCareer.PlanId);
                    studentCareer.StartDate = DateTime.Now;

                    if (plan.Interval == "MES")
                        studentCareer.EndDate = DateTime.Now.AddMonths((int)plan.Increment);
                    else if (plan.Interval == "DIA")
                        studentCareer.EndDate = DateTime.Now.AddDays((int)plan.Increment);

                    studentCareer.StatusId = (int)_SubscriptionStatus.Authorized;
                }
                else if (Action == "No Autorizar")
                {
                    studentCareer.StatusId = (int)_SubscriptionStatus.Canceled;
                }
                else if (Action == "Cancelar")
                {
                    return RedirectToAction("Cancel", "StudentCareers", new { id = studentCareer.Id });
                }

                db.Entry(studentCareer).State = EntityState.Modified;
                db.SaveChanges();

                //Mail

                return RedirectToAction("Index", "StudentCareers");
            }
            ViewBag.CareerId = new SelectList(db.Careers.Where(c => c.Status == true), "CareerId", "CareerName", studentCareer.CareerId);
            ViewBag.PlanId = new SelectList(db.Plans.Where(p => p.Status == true), "PlanId", "PlanName", studentCareer.PlanId);
            //ViewBag.StudentId = new SelectList(db.Students, "StudentId", "UserId", studentCareer.StudentId);
            return View(studentCareer);
        }

        // GET: StudentCareers/Cancel/5
        [Authorize(Roles = "Admin")]
        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCareer studentCareer = db.StudentCareers.Find(id);
            if (studentCareer == null)
            {
                return HttpNotFound();
            }
            return View(studentCareer);
        }

        // POST: StudentCareers/Cancel/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {
            StudentCareer studentCareer = db.StudentCareers.Find(id);
            //db.StudentCareers.Remove(studentCareer);
            studentCareer.StatusId = (int)_SubscriptionStatus.Canceled;
            db.Entry(studentCareer).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Students");
        }

        // GET: StudentCareers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCareer studentCareer = db.StudentCareers.Find(id);
            if (studentCareer == null)
            {
                return HttpNotFound();
            }
            return View(studentCareer);
        }

        // POST: StudentCareers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentCareer studentCareer = db.StudentCareers.Find(id);
            db.StudentCareers.Remove(studentCareer);
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
