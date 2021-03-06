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
    
    public partial class Evaluation
    {
        public Evaluation()
        {
            this.EvaluationDetails = new HashSet<EvaluationDetail>();
            this.EvaluationQuestions = new HashSet<EvaluationQuestion>();
        }
    
        public int EvaluationId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public Nullable<bool> Completed { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<decimal> Score { get; set; }
        public Nullable<int> QuestionsInExam { get; set; }
        public Nullable<decimal> MinScore { get; set; }
        public Nullable<int> CareerId { get; set; }
        public Nullable<bool> IsLastOne { get; set; }
    
        public virtual Subject Subject { get; set; }
        public virtual ICollection<EvaluationDetail> EvaluationDetails { get; set; }
        public virtual ICollection<EvaluationQuestion> EvaluationQuestions { get; set; }
        public virtual Career Career { get; set; }
        public virtual Student Student { get; set; }
    }
}
