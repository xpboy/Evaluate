using Evaluate.Models;
using Evaluate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using PagedList;
using System.Data.Entity;

namespace Evaluate.Controllers
{
    [Authorize]
    [SessionExpire(SessionName = "student")]
    public class EvaluationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Exams
        [HttpGet]
        public ActionResult Exams()
        {
            int studentId = Convert.ToInt32(((Student)Session["student"]).StudentId);

            //var careerId = GetCareerId(studentId);
            //var exams = db.Subjects.Where(s => s.CareerId == careerId && s.Status == true);

            DateTime today = DateTime.Now;
            var exams = from s in db.Subjects
                        join c in db.StudentCareers on s.CareerId equals c.CareerId
                        where c.StudentId == studentId
                            && s.Status == true
                            && c.StatusId == (int)_SubscriptionStatus.Authorized
                            && today >= c.StartDate
                            && today <= c.EndDate
                        select s;
            
            return View(exams.ToList());
        }

        [HttpGet]
        public ActionResult PreIndex(int? careerId, int? subjectId)
        {
            int evaluationId = 0;
            bool correct=false;

            if(careerId==null && subjectId==null)
                return RedirectToAction("Exams");

            while (!correct)
            {
                if (subjectId != null)
                    evaluationId = GetEvaluationIdBySubject((int)((Student)Session["student"]).StudentId, subjectId);
                else if (careerId != null)
                    evaluationId = GetEvaluationIdByCareer((int)((Student)Session["student"]).StudentId, careerId);
                
                correct = IsEvaluationCorrect(evaluationId);
            }

            if (LastQuestion(evaluationId) > 1)
                return RedirectToAction("Reset", "Evaluations", new { id = evaluationId });
            else
                return RedirectToAction("Index", "Evaluations", new { id = evaluationId });
        }

        [HttpGet]
        public ActionResult Reset(int? id)
        {
            var eva = db.Evaluations.Find(id);
            return View(eva);
        }

        [HttpPost]
        public ActionResult Reset(Evaluation eva, string Action)
        {
            if (ModelState.IsValid)
            {
                if (Action == "Reiniciar")
                {
                    eva.Completed = true;
                    db.Entry(eva).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("PreIndex", "Evaluations", new { careerId = eva.CareerId, subjectId = eva.SubjectId });
                }
                else if (Action == "Continuar")
                {
                    return RedirectToAction("Index", "Evaluations", new { id = eva.EvaluationId });
                }
            }

            return View(eva);
        }

        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (id == null)
                return RedirectToAction("Exams");

            //valid exam for student
            var eva = db.Evaluations.Find(id);
            if (eva != null)
            {
                if (eva.StudentId != ((Student)Session["student"]).StudentId)
                    return RedirectToAction("Exams");
            }
            else
                return RedirectToAction("Exams");

            ExamViewModel _exam = GetExam((int)id, LastQuestion((int)id));

            Session["exam"] = _exam;

            ViewBag.SubjectName = _exam.SubjectName;
            return View(_exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ExamViewModel model, string Nxt)
        {
            if (Session["exam"] == null)
                return RedirectToAction("Results", "Evaluations");

            ExamViewModel questionModel = ((ExamViewModel)Session["exam"]);
            ExamViewModel _exam = null;
            int evaluationId = questionModel.EvaluationId;
            int sequenceNumber = 0;// = Convert.ToInt32(((navExam)Session["exam"]).SequenceNumber);
            int? selectedAnswerId = 0;
            foreach (var a in model.Questions)
            {
                selectedAnswerId = a.SelectedAnswer;
            }

            if (selectedAnswerId == null && Nxt == "Siguiente")
                return View(questionModel);

            foreach (var q in questionModel.Questions)
            {
                EvaluationQuestion evaQ = db.EvaluationQuestions.Where(eq => eq.EvaluationId == evaluationId && eq.QuestionId == q.QuestionId).FirstOrDefault();

                sequenceNumber = q.Sequence;
                if (Nxt == "Siguiente")
                {
                    evaQ.AnswerId = selectedAnswerId;
                    evaQ.IsCorrect = VerifyAnswer(q.QuestionId, selectedAnswerId);
                    evaQ.CorrectAnswerId = CorrectAnswerId(q.QuestionId);
                    db.Entry(evaQ).State = EntityState.Modified;
                    db.SaveChanges();

                    //if (questionModel.Questions.Count > sequenceNumber)
                    //{
                    sequenceNumber++;
                    _exam = GetExam(evaluationId, sequenceNumber);
                    //}
                }
                if (Nxt == "Anterior")
                {
                    ModelState.Clear();
                    if (sequenceNumber > 1)
                    {
                        evaQ.AnswerId = selectedAnswerId;
                        evaQ.IsCorrect = VerifyAnswer(q.QuestionId, selectedAnswerId);
                        evaQ.CorrectAnswerId = CorrectAnswerId(q.QuestionId);
                        db.Entry(evaQ).State = EntityState.Modified;
                        db.SaveChanges();

                        sequenceNumber--;
                        _exam = GetExam(evaluationId, sequenceNumber);
                    }
                    else
                        _exam = GetExam(evaluationId, 1);
                }
            }

            var result = db.EvaluationQuestions.Where(e => e.EvaluationId == evaluationId && e.Sequence == sequenceNumber).FirstOrDefault();

            if (result == null)
            {
                VerifyExam(evaluationId, sequenceNumber);

                return RedirectToAction("ResultDetails", "Evaluations", new { id = evaluationId });
            }
            Session["exam"] = _exam;

            ViewBag.SubjectName = _exam.SubjectName;
            return View(_exam);
        }

        [HttpGet]
        public ActionResult Results()
        {
            int studentId = (int)((Student)Session["student"]).StudentId;

            var results = db.Evaluations.Where(e => e.StudentId == studentId
                && e.Completed == true
                && e.IsLastOne == true
                && e.SubjectId != null).OrderByDescending(o=>o.CreatedDate).ToList();

            return View(results);
        }

        [HttpGet]
        public ActionResult ResultDetails(int id)
        {
            Session["exam"] = null;
            ExamViewModel _exam = GetExamVerified(id);

            //Session["exam"] = _exam;

            return View(_exam);
        }

        [ChildActionOnly]
        public ActionResult Result(int? id)
        {
            var evaluation = db.Evaluations.Find(id);
            if (evaluation.SubjectId == null)
                ViewBag.SubjectName = evaluation.Career.CareerName;
            else
                ViewBag.SubjectName = evaluation.Subject.SubjectName;

            return PartialView("_Result", evaluation);
        }

        public ExamViewModel GetExam(int evaluationId, int sequenceNumber)
        {
            int studentId = ((Student)Session["student"]).StudentId;
            var evaluation = db.Evaluations.Find(evaluationId);
            string subjectName;
            if (evaluation.SubjectId == null)
                subjectName = evaluation.Career.CareerName;
            else
                subjectName = evaluation.Subject.SubjectName;

            //var subject = db.Subjects.Find(subjectId);

            //int evaluationId = GetEvaluationId(studentId, subjectId);

            ExamViewModel examVM = new ExamViewModel
            {
                EvaluationId = evaluationId,
                SubjectId = evaluation.SubjectId,
                CareerId = evaluation.CareerId,
                SubjectName = subjectName,
                //Questions = GetQuestions(subjectId, sequenceNumber)
                Questions = (from q in db.Questions
                             join eq in db.EvaluationQuestions on q.QuestionId equals eq.QuestionId
                             where eq.EvaluationId == evaluationId
                                && eq.Sequence == sequenceNumber
                             select new QuestionViewModel
                             {
                                 QuestionId = q.QuestionId,
                                 QuestionName = q.QuestionName,
                                 SelectedAnswer = eq.AnswerId,
                                 Sequence = eq.Sequence,
                                 Answers = (from a in db.Answers
                                            join ea in db.EvaluationDetails on a.AnswerId equals ea.AnswerId
                                            where a.QuestionId == q.QuestionId
                                                 && ea.EvaluationId == evaluationId
                                                 && a.Status == true
                                            select new AnswerViewModel
                                            {
                                                AnswerId = a.AnswerId,
                                                AnswerName = a.AnswerName
                                            }).OrderBy(oa => oa.AnswerId).ToList()
                             }).ToList()
            };

            return examVM;

            //var evalVM = new ExamViewModel();

            ////the below is hardcoded for DEMO. you may get the data from some  
            ////other place and set the questions and answers

            //evalVM.EvaluationId = evaluationId;

            //QuestionViewModel q3 = GetQuestions(subjectId, sequenceNumber).FirstOrDefault();
            //evalVM.Questions.Add(q3);


            ////return View(evalVM);

            //return evalVM;
        }

        public int LastQuestion(int evaluationId)
        {
            var result = db.EvaluationQuestions
                .Where(eq => eq.EvaluationId == evaluationId && eq.AnswerId == null)
                .OrderBy(x => x.Sequence).First();

            return result.Sequence;
        }

        public int CorrectAnswerId(int questionId)
        {
            var correctAnswer = db.Answers.Where(a => a.QuestionId == questionId && a.IsCorrect == true).FirstOrDefault();

            return correctAnswer.AnswerId;
        }

        public void VerifyExam(int evaluationId, int? lastSequenceNumber)
        {
            int? questionsInExam;
            int? correctQuestions = db.EvaluationQuestions.Where(e => e.EvaluationId == evaluationId && e.IsCorrect == true).Count();
            correctQuestions = correctQuestions == null ? 0 : correctQuestions;

            Evaluation eva = db.Evaluations.Find(evaluationId);
            
            db.Evaluations
                .Where(e => e.StudentId == eva.StudentId
                    && e.SubjectId == eva.SubjectId
                    && e.CareerId == eva.CareerId
                    && e.Completed == true).ToList()
                .ForEach(a => a.IsLastOne = false);
            db.SaveChanges();

            questionsInExam = eva.QuestionsInExam > lastSequenceNumber ? lastSequenceNumber : eva.QuestionsInExam;
            eva.Completed = true;
            eva.Score = (correctQuestions * 100) / eva.QuestionsInExam;
            eva.IsLastOne = true;
            db.Entry(eva).State = EntityState.Modified;
            db.SaveChanges();
        }

        public bool VerifyAnswer(int questionId, int? selectedAnswerId)
        {
            bool isCorrect;

            if (selectedAnswerId == CorrectAnswerId(questionId))
                isCorrect = true;
            else
                isCorrect = false;

            return isCorrect;
        }

        public ExamViewModel GetExamVerified(int evaluationId)
        {
            ExamViewModel examVM = new ExamViewModel
            {
                EvaluationId = evaluationId,
                //Questions = GetQuestions(subjectId, sequenceNumber)
                Questions = (from q in db.Questions
                             join eq in db.EvaluationQuestions on q.QuestionId equals eq.QuestionId
                             where eq.EvaluationId == evaluationId
                             //&& eq.Sequence == secuenceNumber
                             select new QuestionViewModel
                             {
                                 QuestionId = q.QuestionId,
                                 QuestionName = q.QuestionName,
                                 SelectedAnswer = (int)eq.AnswerId,
                                 Sequence = eq.Sequence,
                                 IsCorrect = eq.IsCorrect,
                                 CorrectAnswerId = eq.CorrectAnswerId,
                                 Answers = (from a in db.Answers
                                            join ea in db.EvaluationDetails on a.AnswerId equals ea.AnswerId
                                            where a.QuestionId == q.QuestionId
                                                 && ea.EvaluationId == evaluationId
                                                 && a.Status == true
                                            select new AnswerViewModel
                                            {
                                                AnswerId = a.AnswerId,
                                                AnswerName = a.AnswerName
                                            }).OrderBy(oa => oa.AnswerId).ToList()
                             }).OrderBy(o => o.Sequence).ToList()
            };

            return examVM;

            //var evalVM = new ExamViewModel();

            //evalVM.EvaluationId = evaluationId;

            //evalVM.Questions = GetQuestionVerified(evaluationId).ToList(); 

            //return evalVM;
        }

        public int GetCareerId(int studentId)
        {
            DateTime today = DateTime.Now;
            var careerId = (from s in db.StudentCareers
                            where s.StudentId == studentId
                            && s.StatusId == (int)_SubscriptionStatus.Authorized
                            && today >= s.StartDate 
                            && today <= s.EndDate
                            select s.CareerId).FirstOrDefault();

            return (int)careerId;
        }

        public bool IsEvaluationCorrect(int evaluationId)
        {
            bool correct = false;
            //int maxQuestions;

            var eva = db.Evaluations.Find(evaluationId);
            //var car = db.Careers.Find(eva.CareerId);
            //var sub = db.Subjects.Find(eva.SubjectId);

            if (eva.QuestionsInExam == null)
                return false;

            if (eva.CareerId != null)
            {
                if (eva.QuestionsInExam == eva.Career.QuestionsInExam)
                {
                    correct = AreQuestionsCorrects(eva);
                }
                else
                    correct = false;
            }
            else
            {
                if (eva.QuestionsInExam == eva.Subject.QuestionsInExam)
                {
                    correct = AreQuestionsCorrects(eva);
                }
                else
                    correct = false;
            }

            if (!correct)
            {
                eva.Completed = true;
                db.Entry(eva).State = EntityState.Modified;
                db.SaveChanges();
            }

            return correct;
        }

        public bool AreQuestionsCorrects(Evaluation eva)
        {
            bool correct = false;

            var questions = db.EvaluationQuestions.Where(q => q.EvaluationId == eva.EvaluationId);

            var query = db.EvaluationDetails.GroupBy(d => d.QuestionId,
                (key, values) => new { Question = key, Count = values.Count() });


            if (questions.Count() == eva.QuestionsInExam)
            {
                foreach (var d in query)
                {
                    if (d.Count > 0)
                        correct = true;
                    else
                        correct = false;
                }

                //foreach (var q in questions)
                //{
                //    correct = true;
                //}
            }
            else
                correct = false;

            return correct;
        }

        public int GetEvaluationIdBySubject(int studentId, int? subjectId)
        {
            int evaluationId = (from e in db.Evaluations
                                where e.StudentId == studentId
                                && e.SubjectId == subjectId
                                && e.Completed == false
                                select e.EvaluationId).FirstOrDefault();

            if (evaluationId == 0)
            {
                var subject = db.Subjects.Find(subjectId);

                int questionsNumber = (int)subject.QuestionsInExam;

                var questions = (from q in db.Questions.Where(q => q.Answers.Count > 0)
                                 where q.SubjectId == subjectId
                                    && q.Status == true
                                 select q).OrderBy(x => Guid.NewGuid()).Take(questionsNumber).ToList();

                Evaluation e = new Evaluation();
                e.StudentId = studentId;
                e.SubjectId = subjectId; 
                e.CareerId = null;
                e.Completed = false;
                e.CreatedDate = DateTime.Now;
                e.QuestionsInExam = questionsNumber;
                e.MinScore = subject.MinScore;
                e.IsLastOne = false;
                db.Evaluations.Add(e);
                db.SaveChanges();
                evaluationId = e.EvaluationId;

                int _sequesce = 1;
                foreach (var question in questions)
                {
                    EvaluationQuestion q = new EvaluationQuestion();
                    q.EvaluationId = evaluationId;
                    q.QuestionId = question.QuestionId;
                    q.AnswerId = null;
                    q.IsCorrect = null;
                    q.Sequence = _sequesce;
                    db.EvaluationQuestions.Add(q);
                    db.SaveChanges();

                    EvaluationDetail d = new EvaluationDetail();
                    d.EvaluationId = e.EvaluationId;
                    d.QuestionId = question.QuestionId;

                    var answers = (from a in db.Answers
                                   where a.QuestionId == question.QuestionId
                                   && a.Status == true
                                   select a).ToList();

                    //foreach (var answer in question.Answers.Where(a => a.Status == true))
                    foreach (var answer in answers)
                    {
                        d.EvaluationId = evaluationId;
                        d.QuestionId = q.QuestionId;
                        d.AnswerId = answer.AnswerId;
                        d.Selected = null;
                        d.IsCorrect = answer.IsCorrect;
                        db.EvaluationDetails.Add(d);
                        db.SaveChanges();
                    }
                    _sequesce++;
                }
            }

            return evaluationId;
        }

        public int GetEvaluationIdByCareer(int studentId, int? careerId)
        {
            int evaluationId = (from e in db.Evaluations
                                where e.StudentId == studentId
                                && e.SubjectId == null
                                && e.CareerId == careerId
                                && e.Completed == false
                                select e.EvaluationId).FirstOrDefault();

            if (evaluationId == 0)
            {
                var career = db.Careers.Find(careerId);

                int questionsNumber = (int)career.QuestionsInExam;

                var questions = (from s in db.Subjects.Where(s => s.Questions.Count > 0)
                                 join q in db.Questions.Where(q => q.Answers.Count > 0) on s.SubjectId equals q.SubjectId
                                 where s.CareerId == careerId
                                    && s.Status==true
                                    && q.Status == true
                                 select q).OrderBy(x => Guid.NewGuid()).Take(questionsNumber).ToList();

                Evaluation e = new Evaluation();
                e.StudentId = studentId;
                e.SubjectId = null;
                e.CareerId = careerId;
                e.Completed = false;
                e.CreatedDate = DateTime.Now;
                e.QuestionsInExam = questionsNumber;
                e.MinScore = career.MinScore;
                e.IsLastOne = false;
                db.Evaluations.Add(e);
                db.SaveChanges();
                evaluationId = e.EvaluationId;

                int _sequesce = 1;
                foreach (var question in questions)
                {
                    EvaluationQuestion q = new EvaluationQuestion();
                    q.EvaluationId = evaluationId;
                    q.QuestionId = question.QuestionId;
                    q.AnswerId = null;
                    q.IsCorrect = null;
                    q.Sequence = _sequesce;
                    db.EvaluationQuestions.Add(q);
                    db.SaveChanges();

                    EvaluationDetail d = new EvaluationDetail();
                    d.EvaluationId = e.EvaluationId;
                    d.QuestionId = question.QuestionId;

                    foreach (var answer in question.Answers.Where(a => a.Status == true))
                    //foreach (var answer in answers)
                    {
                        d.EvaluationId = evaluationId;
                        d.QuestionId = q.QuestionId;
                        d.AnswerId = answer.AnswerId;
                        d.Selected = null;
                        d.IsCorrect = answer.IsCorrect;
                        db.EvaluationDetails.Add(d);
                        db.SaveChanges();
                    }
                    _sequesce++;
                }
            }

            return evaluationId;
        }

        //public ActionResult Show()
        //{
        //    var evalVM = new Eva();

        //    //the below is hardcoded for DEMO. you may get the data from some  
        //    //other place and set the questions and answers

        //    var q1 = new _Question { ID = 1, QuestionText = "What is your favourite language" };
        //    q1.Answers.Add(new _Answer { ID = 12, AnswerText = "MVC" });
        //    q1.Answers.Add(new _Answer { ID = 13, AnswerText = "ASP.NET" });
        //    q1.Answers.Add(new _Answer { ID = 14, AnswerText = "Java" });
        //    evalVM.Questions.Add(q1);

        //    var q2 = new _Question { ID = 2, QuestionText = "What is your favourite DB" };
        //    q2.Answers.Add(new _Answer { ID = 16, AnswerText = "SQL Server" });
        //    q2.Answers.Add(new _Answer { ID = 17, AnswerText = "EF" });
        //    q2.Answers.Add(new _Answer { ID = 18, AnswerText = "Oracle" });
        //    evalVM.Questions.Add(q2);

        //    return View(evalVM);

        //}
        //[HttpPost]
        //public ActionResult Show(Eva model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var q in model.Questions)
        //        {
        //            var qId = q.ID;
        //            //var selectedAnswer = q.SelectedAnswer;
        //            // Save the data 
        //        }
        //        return RedirectToAction("ThankYou"); //PRG Pattern
        //    }
        //    //to do : reload questions and answers
        //    return View(model);
        //}

        // GET: Evaluations
        //public ActionResult Exam(int? id, int? page)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    page = page == null ? 1 : page;

        //    int pageSize = 1;
        //    int pageNumber = (page ?? 1);

        //    int studentId = ((Student)Session["student"]).StudentId;
        //    int? subjectId = id;

        //    int evaluationId = GetEvaluationId(studentId, subjectId);

        //    var questions = (from q in db.Questions
        //                     join eq in db.EvaluationQuestions on q.QuestionId equals eq.QuestionId
        //                     where eq.EvaluationId == evaluationId
        //                     select new QuestionViewModel
        //                     {
        //                         QuestionId = q.QuestionId,
        //                         QuestionName = q.QuestionName,
        //                         //SelectedAnswer = null,
        //                         Sequence = eq.Sequence
        //                     }).ToList();

        //    List<QuestionViewModel> lQuestions = new List<QuestionViewModel>();
        //    foreach (var q in questions)
        //    {
        //        QuestionViewModel nQ = new QuestionViewModel();
        //        //nQ.EvaluationId = evaluationId;
        //        nQ.QuestionId = q.QuestionId;
        //        nQ.QuestionName = q.QuestionName;
        //        //nQ.SelectedAnswer = null;
        //        nQ.Sequence = q.Sequence;

        //        var answers = (from a in db.Answers
        //                       join ea in db.EvaluationDetails on a.AnswerId equals ea.AnswerId
        //                       where a.QuestionId == q.QuestionId
        //                            && ea.EvaluationId == evaluationId
        //                            && a.Status == true
        //                       select new AnswerViewModel
        //                       {
        //                           AnswerId = a.AnswerId,
        //                           AnswerName = a.AnswerName
        //                       }).ToList();

        //        List<AnswerViewModel> lAnswers = new List<AnswerViewModel>();
        //        foreach (var a in answers)
        //        {
        //            AnswerViewModel nA = new AnswerViewModel();
        //            nA.AnswerId = a.AnswerId;
        //            nA.AnswerName = a.AnswerName;
        //            lAnswers.Add(nA);
        //        }
        //        nQ.Answers = lAnswers;
        //        lQuestions.Add(nQ);
        //    }

        //    if (lQuestions == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(lQuestions.OrderBy(o => o.Sequence).ToPagedList(pageNumber, pageSize));
        //}

        // GET: Evaluations
        //public ActionResult Index2(int? id, string direction)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    int studentId = ((Student)Session["student"]).StudentId;
        //    int? subjectId = id;

        //    int evaluationId = CreateEvaluation(studentId, subjectId);

        //    var questions = (from q in db.Questions
        //                     join eq in db.EvaluationQuestions on q.QuestionId equals eq.QuestionId
        //                     where eq.EvaluationId == evaluationId
        //                     select new QuestionViewModel
        //                     {
        //                         QuestionId = q.QuestionId,
        //                         QuestionName = q.QuestionName,
        //                         //SelectedAnswer = null,
        //                         Sequence = eq.Sequence
        //                     }).ToList();

        //    List<QuestionViewModel> lQuestions = new List<QuestionViewModel>();
        //    foreach (var q in questions)
        //    {
        //        QuestionViewModel nQ = new QuestionViewModel();
        //        //nQ.EvaluationId = evaluationId;
        //        nQ.QuestionId = q.QuestionId;
        //        nQ.QuestionName = q.QuestionName;
        //        //nQ.SelectedAnswer = null;
        //        nQ.Sequence = q.Sequence;

        //        var answers = (from a in db.Answers
        //                       join ea in db.EvaluationDetails on a.AnswerId equals ea.AnswerId
        //                       where a.QuestionId == q.QuestionId
        //                            && ea.EvaluationId == evaluationId
        //                            && a.Status == true
        //                       select new AnswerViewModel
        //                       {
        //                           AnswerId = a.AnswerId,
        //                           AnswerName = a.AnswerName
        //                       }).ToList();

        //        List<AnswerViewModel> lAnswers = new List<AnswerViewModel>();
        //        foreach (var a in answers)
        //        {
        //            AnswerViewModel nA = new AnswerViewModel();
        //            nA.AnswerId = a.AnswerId;
        //            nA.AnswerName = a.AnswerName;
        //            lAnswers.Add(nA);
        //        }
        //        nQ.Answers = lAnswers;
        //        lQuestions.Add(nQ);
        //    }

        //    if (lQuestions == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(lQuestions.OrderBy(o => o.Sequence));
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Exam(IPagedList<QuestionViewModel> model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        EvaluationQuestion eQuestion = new EvaluationQuestion();
        //        foreach (var q in model)
        //        {
        //            var qId = q.QuestionId;
        //            //var selectedAnswer = q.SelectedAnswer;


        //            //db.Entry(questions).State = EntityState.Modified;
        //            //db.SaveChanges();
        //        }
        //        return RedirectToAction("ThankYou"); //PRG Pattern
        //    }
        //    //if (ModelState.IsValid)
        //    //{
        //    //    db.Entry(career).State = EntityState.Modified;
        //    //    db.SaveChanges();
        //    //    return RedirectToAction("Index");
        //    //}

        //    //to do : reload questions and answers
        //    return View(model);
        //}



        //[HttpPost]
        //public ActionResult Index(Evaluation model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = new Evaluation();
        //        result.StudentId = 1;
        //        result.SubjectId = 1;
        //        result.Completed = true;
        //        result.CreatedDate = DateTime.Now;
        //        db.Evaluations.Add(result);

        //        foreach (var q in model.Questions)
        //        {
        //            //var qId = q.QuestionId;
        //            //var selectedAnswer = q.SelectedAnswer;
        //            // Save the data 

        //            //Question question = db.Questions.Find(q.QuestionId);
        //            //var icCorrect = (from r in db.Answers
        //            //                 where r.QuestionId == q.QuestionId
        //            //                 && r.AnswerId == Convert.ToInt32(q.SelectedAnswer)
        //            //                 select r.IsCorrect).FirstOrDefault();                 

        //            var isCorrect = db.Answers.Where(r => r.QuestionId == q.QuestionId && r.AnswerId == Convert.ToInt32(q.SelectedAnswer));

        //            var resultDetails = new ResultDetail();
        //            resultDetails.ResultId = result.ResultId;
        //            resultDetails.QuestionId = q.QuestionId;
        //            resultDetails.AnswerId = Convert.ToInt32(q.SelectedAnswer);

        //            resultDetails.IsCorrect = true;// Convert.ToBoolean(icCorrect);
        //            db.ResultDetails.Add(resultDetails);

        //            //db.SaveChanges();
        //        }
        //        return RedirectToAction("Index"); //PRG Pattern
        //        //return RedirectToAction("Results", "Evaluation", new { Id = result.ResultId });
        //    }
        //    //reload questions
        //    return View(model);
        //}

        //public ActionResult Result(int? Id)
        //{
        //    //Question1
        //    int? subjectId = Id;
        //    var items = new Evaluation
        //    {
        //        Questions = (from p in db.Questions
        //                     where p.SubjectId == subjectId
        //                     select new QuestionViewModel
        //                     {
        //                         QuestionId = p.QuestionId,
        //                         Question = p.QuestionName,
        //                         SelectedAnswer = null,
        //                         Answers = (from r in db.Answers
        //                                    where r.QuestionId == p.QuestionId
        //                                    select new AnswerViewModel
        //                                    {
        //                                        AnswerId = r.AnswerId,
        //                                        Answer = r.AnswerName
        //                                    }).ToList()
        //                     }).ToList()
        //    };

        //    return View(items);
        //}
    }
}

//var items = new Evaluation
//{
//    Questions = (from p in db.Questions
//                 where p.SubjectId == subjectId
//                    && p.Status == true
//                 select new QuestionViewModel
//                 {
//                     QuestionId = p.QuestionId,
//                     Question = p.QuestionName,
//                     SelectedAnswer = null,
//                     Answers = (from r in db.Answers
//                                where r.QuestionId == p.QuestionId
//                                    && r.Status == true
//                                select new AnswerViewModel
//                                {
//                                    AnswerId = r.AnswerId,
//                                    Answer = r.AnswerName
//                                }).ToList()
//                 }).OrderBy(x => Guid.NewGuid()).Take(50).ToList()
//};

//var q1 = new QuestionViewModel { QuestionId = 101, QuestionName = "What is your favourite language", Sequence = 101 };
//q1.Answers.Add(new AnswerViewModel { AnswerId = 102, AnswerName = "MVC" });
//q1.Answers.Add(new AnswerViewModel { AnswerId = 103, AnswerName = "ASP.NET" });
//q1.Answers.Add(new AnswerViewModel { AnswerId = 104, AnswerName = "Java" });
//evalVM.Questions.Add(q1);

//var q2 = new QuestionViewModel { QuestionId = 102, QuestionName = "What is your favourite DB", Sequence = 102 };
//q2.Answers.Add(new AnswerViewModel { AnswerId = 106, AnswerName = "SQL Server" });
//q2.Answers.Add(new AnswerViewModel { AnswerId = 107, AnswerName = "EF" });
//q2.Answers.Add(new AnswerViewModel { AnswerId = 108, AnswerName = "Oracle" });
//evalVM.Questions.Add(q2);