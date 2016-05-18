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
    [Authorize(Roles = "Admin")]
    public class AnswersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Answers
        public ActionResult Index(int? questionId)
        {
            if (questionId == null)
            {
                if (Session["question"] == null)
                    return RedirectToAction("Index", "Questions");
                else
                    questionId = ((Question)Session["question"]).QuestionId;
            }
            else
            {
                //if (Session["question"] != null)
                //{
                //    if (((Question)Session["question"]).QuestionId != questionId)
                //    {
                //        var question = db.Questions.Find(questionId);
                //        Session["question"] = question;
                //    }
                //}
                //else
                //{
                var question = db.Questions.Find(questionId);
                Session["question"] = question;
                //}
            }

            var answers = db.Answers.Where(q => q.QuestionId == questionId);
            ViewBag.Question = ((Question)Session["question"]).QuestionName;
            ViewBag.QuestionId = questionId;

            return View(answers.ToList());

            //var answers = db.Answers;
            //return View(answers.ToList());
        }

        // GET: Answers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: Answers/Create
        public ActionResult Create()
        {
            ViewBag.QuestionName = ((Question)Session["question"]).QuestionName;
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnswerId,QuestionId,AnswerName,IsCorrect,Status")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                answer.QuestionId = ((Question)Session["question"]).QuestionId;
                if (answer.Status == null)
                    answer.Status = true;
                db.Answers.Add(answer);
                db.SaveChanges();
                return RedirectToAction("Index", new { questionId = answer.QuestionId });
            }

            return View(answer);
        }

        // GET: Answers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestionName = ((Question)Session["question"]).QuestionName;
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnswerId,QuestionId,AnswerName,IsCorrect,Status")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                answer.QuestionId = ((Question)Session["question"]).QuestionId;

                if (answer.Status == null)
                    answer.Status = true;
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { questionId = answer.QuestionId });
            }
            //ViewBag.QuestionId = new SelectList(db.Questions, "QuestionId", "QuestionName", answer.QuestionId);
            return View(answer);
        }

        // GET: Answers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer answer = db.Answers.Find(id);
            db.Answers.Remove(answer);
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
