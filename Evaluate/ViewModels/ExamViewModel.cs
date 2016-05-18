using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class _Answer
    {
        public int ID { set; get; }
        public string AnswerText { set; get; }
    }

    public class Eva
    {
        public List<_Question> Questions { set; get; }
        public Eva()
        {
            Questions = new List<_Question>();
        }
    }

    public class _Question
    {
        public int ID { set; get; }
        public string QuestionText { set; get; }
        public List<_Answer> Answers { set; get; }
        public string SelectedAnswer { set; get; }
        public _Question()
        {
            Answers = new List<_Answer>();
        }
    }
    //public class AnswerViewModel
    //{
    //    public int ID { set; get; }
    //    public string AnswerText { set; get; }
    //}

    //public class Exam
    //{
    //    public List<QuestionViewModel> Questions { set; get; }
    //    public Exam()
    //    {
    //        Questions = new List<QuestionViewModel>();
    //    }
    //}

    //public class QuestionViewModel
    //{
    //    public int ID { set; get; }
    //    public string QuestionText { set; get; }
    //    public List<AnswerViewModel> Answers { set; get; }
    //    public string SelectedAnswer { set; get; }
    //    public QuestionViewModel()
    //    {
    //        Answers = new List<AnswerViewModel>();
    //    }
    //}

    public class QuestionViewModel
    {
        //public int Id { get; set; }
        //public int EvaluationId { get; set; }
        public int QuestionId { set; get; }
        public string QuestionName { set; get; }
        public List<AnswerViewModel> Answers { set; get; }
        [Required]
        public int? CorrectAnswerId { get; set; }

        [Required(ErrorMessage="Debe seleccionar una respuesta")]
        public int? SelectedAnswer { set; get; }
        public int Sequence { get; set; }
        public QuestionViewModel()
        {
            Answers = new List<AnswerViewModel>();
        }
        public bool? IsCorrect { get; set; }
    }
    public class AnswerViewModel
    {
        public int AnswerId { set; get; }
        public string AnswerName { set; get; }
        //public string IsCorrect { get; set; }
    }
    public class ExamViewModel
    {
        public int EvaluationId { get; set; }
        public int? SubjectId { get; set; }
        public int? CareerId { get; set; }
        public List<QuestionViewModel> Questions { set; get; }
        public bool Finished { get; set; }
        public string SubjectName { get; set; }
        public ExamViewModel()
        {
            Questions = new List<QuestionViewModel>();
        }
    }

    //public class ExamQuesionsViewModel
    //{
    //    public int EvaluationId { get; set; }
    //    public int QuestionId { get; set; }
    //}

    //public class EvaluationResult
    //{
    //    public List<QuestionViewModel> Questions { set; get; }
    //    public EvaluationResult()
    //    {
    //        Questions = new List<QuestionViewModel>();
    //    }
    //}

    //public class EvaluationViewModel
    //{
    //    public int SubjectId { get; set; }
    //    public string Subject { get; set; }
    //    public int QuestionId { get; set; }
    //    public string Question { get; set; }
    //    public int AnswerId { get; set; }
    //    public string Answer { get; set; }
    //    public bool IsCorrect { get; set; }
    //}
}