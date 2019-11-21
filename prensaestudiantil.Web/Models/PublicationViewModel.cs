using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using prensaestudiantil.Web.Data.Entities;

namespace prensaestudiantil.Web.Models
{
    public class PublicationViewModel : Publication
    {
        public string UserId { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "The field {0} is mandatory.")]
        public int PublicationCategoryId { get; set; }

        public IEnumerable<SelectListItem> PublicationCategories { get; set; }
    }

}
