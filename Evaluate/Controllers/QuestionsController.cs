using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Evaluate.Models;
using System.IO;
using System.Drawing;

namespace Evaluate.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Questions
        public ActionResult Index(int? subjectId)
        {
            if (subjectId == null)
            {
                if (Session["subject"] == null)
                    return RedirectToAction("Index", "Subjects");
                else
                    subjectId = ((Subject)Session["subject"]).SubjectId;
            }
            else
            {
                var subject = db.Subjects.Find(subjectId);
                Session["subject"] = subject;
                //}
            }

            var questions = db.Questions.Where(q => q.SubjectId == subjectId);
            ViewBag.Subject = ((Subject)Session["subject"]).SubjectName;
            ViewBag.SubjectId = subjectId;

            return View(questions.ToList());

            //var questions = db.Questions.Include(q => q.Subject);
            //return View(questions.ToList());
        }

        //// GET: Questions/Answers
        //public ActionResult Answers(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var answers = db.Answers.Where(a => a.QuestionId == id).Include(a=>a.Question);
        //    if (answers == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(answers.ToList());
        //}

        //// GET: Questions/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Question question = db.Questions.Find(id);
        //    if (question == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(question);
        //}

        // GET: Questions/Create
        public ActionResult Create()
        {
            //ViewBag.Id = ((Subject)Session["subject"]).SubjectId;
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuestionId,SubjectId,QuestionName,Difficulty,Status")] Question question, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                question.SubjectId = ((Subject)Session["subject"]).SubjectId;
                if (question.Status == null)
                    question.Status = true;
                question.ImageName = SaveImage(question.QuestionId.ToString(), file);
                db.Questions.Add(question);
                db.SaveChanges();
                
                //if (file.ContentLength > 0)
                //{
                //    string relativePath = "~/images/questions/" + Path.GetFileName(file.FileName);
                //    string physicalPath = Server.MapPath(relativePath);
                //    file.SaveAs(physicalPath);
                //    return View((object)relativePath);
                //}

                return RedirectToAction("Index", new { subjectId = question.SubjectId });
            }
            
            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.Subject = ((Subject)Session["subject"]).SubjectName;

            //var file = Directory.GetFiles("~/images/questions/", question.QuestionId + ".*");
            //string physicalPath = "~/images/questions/" + question.QuestionId + ".jpg";

            //string physicalPath = Directory.GetFiles("~/images/questions/", question.QuestionId + ".*");

            ViewBag.Image = GetPath() + question.ImageName;// "~/images/questions/" + question.QuestionId;

            //string img = SetImagen(question.QuestionId.ToString());
            return View(question);
        }

        //public void SetImagen(string name)
        //{
        //    Image img;  

        //    //string serverFile = GetRequestsDocumentsServerPath() + name;
        //    string serverFile = Server.MapPath(GetRequestsDocumentsServerPath() + name);
        //    string appFile = Server.MapPath(GetRequestsDocumentsServerPath() + name);
        //    //System.IO.File.Copy(serverFile, appFile, true);

        //    img = Directory.GetFiles() GetRequestsDocumentsServerPath() + name;
        //    //img.ImageUrl = GetRequestsDocumentsPath() + "a1050703-1c7c-46cf-b030-203f806dc1aa.jpg";
        //    //return img;
        //}

        public static string GetPath()
        {
            return "~/images/questions/";
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionId,SubjectId,QuestionName,Difficulty,Status")] Question question, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                question.SubjectId = ((Subject)Session["subject"]).SubjectId;

                if (question.Status == null)
                    question.Status = true;
                if(file != null)
                    question.ImageName = SaveImage(question.QuestionId.ToString(), file); 
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();

                //SaveImage(question.QuestionId.ToString(), file);

                return RedirectToAction("Index", new { questionId = question.SubjectId });
            }
            //ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", question.SubjectId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
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

        protected string SaveImage(string fileName, HttpPostedFileBase file)
        {
            string fullFileName = string.Empty;
            if (file != null)
            {
                //fileName = question.QuestionId.ToString();// System.IO.Path.GetFileName(file.FileName);
                string ext = System.IO.Path.GetExtension(file.FileName);
                fullFileName = fileName + ext;
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/images/questions"), fullFileName);

                // file is uploaded
                file.SaveAs(path);

                //// save the image path path to the database or you can send image
                //// directly to database
                //// in-case if you want to store byte[] ie. for DB
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    file.InputStream.CopyTo(ms);
                //    byte[] array = ms.GetBuffer();
                //}
            }

            return fullFileName;
        }
    }
}

//http://www.prideparrot.com/blog/archive/2012/8/uploading_and_returning_files
//public ActionResult Upload(HttpPostedFileBase photo)
//{
//    if (photo != null && photo.ContentLength > 0)
//    {
//        string directory = @"D:\Temp\";
     
//        if (photo.ContentLength > 10240)
//        {
//            ModelState.AddModelError("photo", "The size of the file should not exceed 10 KB");
//            return View();
//        }
 
//        var supportedTypes = new[] { "jpg", "jpeg", "png" };
 
//        var fileExt = System.IO.Path.GetExtension(photo.FileName).Substring(1);
 
//        if (!supportedTypes.Contains(fileExt))
//        {
//            ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
//            return View();
//        }
 
//        var fileName = Path.GetFileName(photo.FileName);
//        photo.SaveAs(Path.Combine(directory, fileName));
//    }
 
//    return RedirectToAction("Index");
//}