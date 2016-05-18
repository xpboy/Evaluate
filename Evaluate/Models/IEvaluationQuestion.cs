using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(EvaluationQuestionMetadata))]
    public partial class EvaluationQuestion { }

    public class EvaluationQuestionMetadata
    {
        public int Sequence { get; set; }
        public bool IsCorrect { get; set; }
    }
}

