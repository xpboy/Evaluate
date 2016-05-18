using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Evaluate.Models;
using Evaluate.ViewModels;

namespace Evaluate.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subjects
        public ActionResult Index(int? careerId)
        {
            if (careerId == null)
            {
                if (Session["navigation"] == null)
                    return RedirectToAction("Index", "Careers");
                else
                    careerId = ((Navigation)Session["navigation"]).CareerId;
            }
            else
            {
                var career = db.Careers.Find(careerId);
                Session["navigation"] = LoadNavigation.LoadCareer(career, (Navigation)Session["navigation"]); ;
            }

            var subjects = db.Subjects.Where(q => q.CareerId == careerId);
            ViewBag.TopName = ((Navigation)Session["navigation"]).CareerName;
            //ViewBag.TopId = careerId;

            return View(subjects.ToList());

            //return View(db.Subjects.ToList());
        }

        //// GET: Subjects/Questions
        //public ActionResult Questions(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var questions = db.Questions.Where(q => q.SubjectId == id);
        //    if (questions == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(db.Subjects.ToList());
        //}
        
        // GET: Subjects/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Subject subject = db.Subjects.Find(id);
        //    if (subject == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(subject);
        //}

        // GET: Subjects/Create
        public ActionResult Create()
        {
            ViewBag.TopId = ((Navigation)Session["navigation"]).CareerId;
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubjectId,SubjectName,Status,QuestionsInExam,MinScore")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.CareerId = ((Navigation)Session["navigation"]).CareerId;
                if (subject.Status == null)
                    subject.Status = true;
                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subject);
        }

        // GET: Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            ViewBag.TopName = ((Navigation)Session["navigation"]).CareerName;
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubjectId,SubjectName,Status,QuestionsInExam,MinScore")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.CareerId = ((Navigation)Session["navigation"]).CareerId;

                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { careerId = subject.CareerId });
            }
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
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
