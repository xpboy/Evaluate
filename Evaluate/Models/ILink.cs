using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    [MetadataTypeAttribute(typeof(LinkMetaData))]
    public partial class Link { }

    public class LinkMetaData
    {
        //public int LinkId { get; set; }

        [Display(Name = "Enlace")]
        public string LinkName { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Url")]
        public string Url { get; set; }

        public string ImageName { get; set; }

        [Display(Name = "Estado")]
        public Nullable<bool> Status { get; set; }
    }
}