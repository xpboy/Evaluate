//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Evaluate.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EvaluationDetail
    {
        public int DetailId { get; set; }
        public Nullable<int> EvaluationId { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public Nullable<int> AnswerId { get; set; }
        public Nullable<bool> Selected { get; set; }
        public Nullable<bool> IsCorrect { get; set; }
    
        public virtual Answer Answer { get; set; }
        public virtual Evaluation Evaluation { get; set; }
        public virtual Question Question { get; set; }
    }
}