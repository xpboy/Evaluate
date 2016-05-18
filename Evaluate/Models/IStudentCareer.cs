using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(StudentCareerMetaData))]
    public partial class StudentCareer { }

    public class StudentCareerMetaData
    {
        public int Id { get; set; }

        [Display(Name = "Estudiante")]
        public Nullable<int> StudentId { get; set; }

        [Display(Name = "Carrera")]
        public Nullable<int> CareerId { get; set; }
        public Nullable<int> CreatedBy { get; set; }

        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> CreatedDate { get; set; }

        [Display(Name="Estado")]
        public Nullable<int> StatusId { get; set; }

        [Display(Name = "Subscripción")]
        public Nullable<int> PlanId { get; set; }

        [Display(Name = "Meses")]
        public Nullable<int> Months { get; set; }

        [Display(Name = "Inicio")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Display(Name = "Fin")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(Name = "Precio")]
        public Nullable<decimal> Price { get; set; }
    }

    public enum _SubscriptionStatus
    {
        Canceled = 0,
        Requested = 1,
        Authorized = 2
    }

    public class SubscriptionsViewModel
    {
        public string UserId { get; set; }

        [Required]
        //[EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name = "Estudiante")]
        public string FullName
        {
            get { return Name + " " + LastName; }
        }

        [Display(Name = "Carrera")]
        public string CareerName { get; set; }

        [Display(Name = "Inicio")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Display(Name = "Fin")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(Name = "Fecha")]
        public string ValidTime
        {
            get { return Convert.ToDateTime(StartDate).ToShortDateString() + " - " + Convert.ToDateTime(EndDate).ToShortDateString(); }
        }

        [Display(Name = "Estado")]
        public Nullable<int> StatusId { get; set; }

        [Display(Name = "Subscripción")]
        public string PlanName { get; set; }

        //public int StudentId { get; set; }
        //public int CareerId { get; set; }
        public int StudentCareerId { get; set; }
    }

}