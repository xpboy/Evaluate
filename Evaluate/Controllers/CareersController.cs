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
    //[Authorize(Roles="Admin")]
    public class CareersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Careers/
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Careers.ToList());
        }

        //// GET: /Careers/Subjects
        //public ActionResult Subjects(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var subjects = db.Subjects.Where(q => q.CareerId == id);
        //    if (subjects == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(subjects.ToList());
        //}

        //// GET: /Careers/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Career career = db.Careers.Find(id);
        //    if (career == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(career);
        //}

        // GET: /Careers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Careers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CareerId,CareerName,Status,QuestionsInExam,MinScore")] Career career)
        {
            if (ModelState.IsValid)
            {
                if (career.Status == null)
                    career.Status = true;
                db.Careers.Add(career);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(career);
        }

        // GET: /Careers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Career career = db.Careers.Find(id);
            if (career == null)
            {
                return HttpNotFound();
            }
            return View(career);
        }

        // POST: /Careers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CareerId,CareerName,Status,QuestionsInExam,MinScore")] Career career)
        {
            if (ModelState.IsValid)
            {
                db.Entry(career).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(career);
        }

        // GET: /Careers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Career career = db.Careers.Find(id);
            if (career == null)
            {
                return HttpNotFound();
            }
            return View(career);
        }

        // POST: /Careers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Career career = db.Careers.Find(id);
            db.Careers.Remove(career);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Student")]
        [ChildActionOnly]
        public ActionResult Exams()
        {
            int studentId = Convert.ToInt32(((Student)Session["student"]).StudentId);
            DateTime today = DateTime.Now;

            //var careerExams = db.Careers.Where(c => c.CareerId == 1).ToList();
            var careerExams = from sc in db.StudentCareers
                              join c in db.Careers on sc.CareerId equals c.CareerId
                              where sc.StudentId == studentId
                                && sc.StatusId == (int)_SubscriptionStatus.Authorized
                                && today >= sc.StartDate
                                && today <= sc.EndDate
                              select c;

            //var users = db.Careers
            //    .Where(c => c.CareerName
            //        .Any(y => y.Equals(db.StudentCareers
            //            .Where(a => a.StudentId == ((Student)Session["student"]).StudentId)
            //            .Select(b => b.Id).FirstOrDefault()))).ToList();

            return PartialView("_Exams", careerExams.ToList());
        }

        [Authorize(Roles = "Student")]
        [ChildActionOnly]
        [SessionExpire(SessionName = "student")]
        public ActionResult Results()
        {
            int studentId = (int)((Student)Session["student"]).StudentId;

            var results = db.Evaluations.Where(e => e.StudentId == studentId
                && e.Completed == true
                && e.IsLastOne == true
                && e.SubjectId == null).ToList();
            return PartialView("_Results", results);
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
