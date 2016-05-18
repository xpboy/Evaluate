using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(SubjectMetadata))]
    public partial class Subject { }

    public class SubjectMetadata
    {
        public int SubjectId { get; set; }

        [Required]
        [Display(Name = "Materia")]
        public string SubjectName { get; set; }

        [Required]
        [Display(Name = "Preguntas por Examen")]
        public Nullable<int> QuestionsInExam { get; set; }

        [Required]
        [Display(Name = "Puntuación Mínima")]
        public Nullable<int> MinScore { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public Nullable<bool> Status { get; set; }
    }
}