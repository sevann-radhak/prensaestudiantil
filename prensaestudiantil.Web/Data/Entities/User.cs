using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prensaestudiantil.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Primer Nombre")]
        [MaxLength(60, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(60, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string LastName { get; set; }

        [Display(Name = "Usuario Activo?")]
        public bool IsEnabled { get; set; }

        [Display(Name = "Foto")]
        public string ImageUrl { get; set; }

        // Only read
        [Display(Name = "Nombre")]
        public string FullName => $"{FirstName} {LastName}";

        // TODO: Change the path when publish
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
             ? $"https://prensaestudiantil.azurewebsites.net/images/Users/noImage.png"
             : $"https://prensaestudiantil.azurewebsites.net{ImageUrl.Substring(1)}";

        [NotMapped]
        [Display(Name = "Roles")]
        public ICollection<string> Roles { get; set; }

        [NotMapped]
        [Display(Name = "Es Administrador?")]
        public bool IsManager { get; set; }

        // Foreing keys
        public ICollection<Publication> Publications { get; set; }

        public ICollection<YoutubeVideo> YoutubeVideos { get; set; }
    }
}
