using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prensaestudiantil.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "First Name")]
        [MaxLength(60, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(60, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string LastName { get; set; }

        [Display(Name = "Is Enabled?")]
        public bool IsEnabled { get; set; }

        [Display(Name = "Main Image")]
        public string ImageUrl { get; set; }

        // Only read
        [Display(Name = "Name")]
        public string FullName => $"{FirstName} {LastName}";

        // TODO: Change the path when publish
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
             ? $"https://prensaestudiantil.azurewebsites.net/images/Users/noImage.png"
             : $"https://prensaestudiantil.azurewebsites.net{ImageUrl.Substring(1)}";

        [NotMapped]
        [Display(Name = "Roles")]
        public ICollection<string> Roles { get; set; }

        [NotMapped]
        [Display(Name = "Is Admin?")]
        public bool IsManager { get; set; }

        // Foreing keys
        public ICollection<Publication> Publications { get; set; }

        public ICollection<YoutubeVideo> YoutubeVideos { get; set; }
    }
}
