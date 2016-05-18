using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(PlanMetaData))]
    public partial class Plan { }

    public class PlanMetaData
    {
        //public int PlanId { get; set; }

        [Required]
        [Display(Name = "Subscripción")]
        public string PlanName { get; set; }
        public Nullable<int> Months { get; set; }

        [Required]
        [Display(Name = "Precio RD$")]
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> Status { get; set; }

        //[Display(Name = "Periodo")]
        //public string PriceCurrency
        //{
        //    get { return "RD$ " + " " + Price; }
        //}

        [Required]
        [Display(Name = "Periodo")]
        public string Interval { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public Nullable<int> Increment { get; set; }
    }
}