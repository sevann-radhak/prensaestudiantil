using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Data.Entities
{
    public class YoutubeVideo
    {
        public int Id { get; set; }

        [Display(Name = "URL")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "The field {0} needs to have 11 characters")]
        public string URL { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Name { get; set; }

        // Foreing key
        public Writer Writer { get; set; }
    }
}
    