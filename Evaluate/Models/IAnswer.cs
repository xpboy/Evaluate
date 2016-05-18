using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(AnswerMetadata))]
    public partial class Answer { }

    public class AnswerMetadata
    {
        public int AnswerId { get; set; }
        public Nullable<int> QuestionId { get; set; }

        [Display(Name = "Respuesta")]
        public string AnswerName { get; set; }

        [Display(Name = "¿Correcta?")]
        [OnlyOneAnswer(ErrorMessage = "Ya existe una respuesta correcta para esta pregunta.")]
        public Nullable<bool> IsCorrect { get; set; }

        [Display(Name = "Estado")]
        public Nullable<bool> Status { get; set; }

    }
}