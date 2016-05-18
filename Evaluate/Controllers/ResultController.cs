using Evaluate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace Evaluate.Controllers
{
    public class ResultController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Result/
        public ActionResult Index(int? Id)
        {
            //int? subjectId = Id;
            //var items = new Exam
            //{
            //    Questions = (from p in db.Questions
            //                 where p.SubjectId == subjectId
            //                 select new QuestionViewModel
            //                 {
            //                     QuestionId = p.QuestionId,
            //                     Question = p.QuestionName,
            //                     SelectedAnswer = null,
            //                     Answers = (from r in db.Answers
            //                                where r.QuestionId == p.QuestionId
            //                                select new AnswerViewModel
            //                                {
            //                                    AnswerId = r.AnswerId,
            //                                    Answer = r.AnswerName
            //                                }).ToList()
            //                 }).ToList()
            //};

            return View();
        }
	}
}