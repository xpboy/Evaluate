using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(PersonMetadata))]
    public partial class Person { }

    public class PersonMetadata
    {
        public int PersonId { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Segundo Nombre")]
        public string SecondName { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name = "Segundo Apellido")]
        public string SecondLastName { get; set; }

        public string Gender { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}