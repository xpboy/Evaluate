using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(EvaluationMetadata))]
    public partial class Evaluation { }

    public class EvaluationMetadata
    {
        public int EvaluationId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> CareerId { get; set; }
        public Nullable<int> SubjectId { get; set; }

        [Display(Name = "Terminado")]
        public Nullable<bool> Completed { get; set; }

        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> CreatedDate { get; set; }

        [Display(Name = "Puntuación")]
        public Nullable<decimal> Score { get; set; }

        [Display(Name = "Preguntas")]
        public Nullable<int> QuestionsInExam { get; set; }

        [Display(Name = "Puntuación Mínima")]
        public Nullable<decimal> MinScore { get; set; }
    }
}