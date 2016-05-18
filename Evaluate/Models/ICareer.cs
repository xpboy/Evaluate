using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(CareerMetaData))]
    public partial class Career { }

    public class CareerMetaData
    {
        public int CareerId { get; set; }

        [Required]
        [Display(Name = "Carrera")]
        public string CareerName { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public Nullable<bool> Status { get; set; }

        [Required]
        [Display(Name = "Preguntas por Examen")]
        public Nullable<int> QuestionsInExam { get; set; }

        [Required]
        [Display(Name = "Puntuación Mínima")]
        public Nullable<int> MinScore { get; set; }
    }
}