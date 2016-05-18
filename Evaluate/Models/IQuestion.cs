using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(QuestionMetadata))]
    public partial class Question { }

    public class QuestionMetadata
    {
        public int QuestionId { get; set; }
        public Nullable<int> SubjectId { get; set; }

        [Display(Name = "Pregunta")]
        public string QuestionName { get; set; }
        public Nullable<int> Difficulty { get; set; }

        [Display(Name = "Estado")]
        public Nullable<bool> Status { get; set; }

        [Display(Name = "Imagen")]
        public string ImageName { get; set; }

    }
}