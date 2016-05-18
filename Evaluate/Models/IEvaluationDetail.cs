using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(EvaluationDetailMetadata))]
    public partial class EvaluationDetail { }

    public class EvaluationDetailMetadata
    {
        [Key]
        public int DetailId { get; set; }
    }
}