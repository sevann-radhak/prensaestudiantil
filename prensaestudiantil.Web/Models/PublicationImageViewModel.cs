using Microsoft.AspNetCore.Http;
using prensaestudiantil.Web.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prensaestudiantil.Web.Models
{
    public class PublicationImageViewModel
    {
        [Display(Name = "Publication")]
        public string PublicationTitle { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public int PublicationId { get; set; }

        public IList<PublicationImage> PublicationImages { get; set; }
    }
}
