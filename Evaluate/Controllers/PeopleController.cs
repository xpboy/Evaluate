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
    public class PeopleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: People
        public ActionResult Index()
        {
            return View(db.People.ToList());
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        //[AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonId,Name,SecondName,LastName,SecondLastName,Gender,CreatedBy,CreatedDate")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.CreatedDate = DateTime.Now;
                db.People.Add(person);
                db.SaveChanges();

                ////var userID = User.Identity.GetUserId();
                //ApplicationUser user = db.Users
                //.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                //var account = new AccountController();

                ////AspNetUser user = db.AspNetUser
                ////    .Where(u => u.UserName.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                //user.PersonId = person.PersonId;
                //db.AspNetUser.Add(user);
                //db.SaveChanges();

                //Student student = new Student();
                //student.PersonId = person.PersonId;
                //student.Status = false;
                //student.CreatedDate = DateTime.Now;
                //db.Students.Add(student);
                //db.SaveChanges();

                //StudentCareer studentCareer = new StudentCareer();
                //studentCareer.StudentId = student.StudentId;
                //studentCareer.CareerId = 1; //CareerId
                //studentCareer.CreatedDate = DateTime.Now;
                //studentCareer.Status = true;
                //db.StudentCareers.Add(studentCareer);
                //db.SaveChanges();

                return RedirectToAction("Waiting", "Main");
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonId,Name,SecondName,LastName,SecondLastName,Gender,CreatedBy,CreatedDate")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
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
