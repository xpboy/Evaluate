using Evaluate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModels;

namespace Evaluate.ViewModels
{
    public class Navigation
    {
        public int CareerId { get; set; }
        public string CareerName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public int AnswerId { get; set; }
        public string AnswerName { get; set; }
    }

    public class LoadNavigation
    {
        public static Navigation LoadCareer(Career career, Navigation nav)
        {
            Navigation newNav = new Navigation();
            if (nav != null)
                newNav = nav;

            newNav.CareerId = career.CareerId;
            newNav.CareerName = career.CareerName;

            return newNav;
        }

        public Navigation LoadSubject(Subject subject, Navigation nav)
        {
            nav.SubjectId = subject.SubjectId;
            nav.SubjectName = subject.SubjectName;

            return nav;
        }
    }

    public class navExam
    {
        public int SubsectId { get; set; }
        public int EvaluationId { get; set; }
        public int SequenceNumber { get; set; }

        public navExam LoadNavExam(ExamViewModel _exam)
        {
            navExam nav = new navExam();
            foreach (var q in _exam.Questions)
            {
                if (q.QuestionId < 100)
                {
                    nav.SubsectId = 1;
                    nav.EvaluationId = 1;
                    nav.SequenceNumber = q.Sequence;
                }
            }
            return nav;
        }
    }
}