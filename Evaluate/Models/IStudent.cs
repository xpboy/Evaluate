using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(StudentMetaData))]
    public partial class Student { }

    public class StudentMetaData
    {
        public int StudentId { get; set; }
        public string UserId { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }

        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}