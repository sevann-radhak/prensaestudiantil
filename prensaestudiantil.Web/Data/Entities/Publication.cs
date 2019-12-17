using prensaestudiantil.Web.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prensaestudiantil.Web.Data.Entities
{
    public class Publication 
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(150, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Title { get; set; }

        [Display(Name = "Encabezado")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DataType(DataType.MultilineText)]
        public string Header { get; set; }

        [Display(Name = "Cuerpo")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Display(Name = "Pie de página")]
        [MaxLength(250, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Footer { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Actualización")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? LastUpdate { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        [Display(Name = "Descripción Imagen")]
        [MaxLength(150, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string ImageDescription { get; set; }

        // Only for external references for author
        [Display(Name = "Autor")]
        [MaxLength(60, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Author { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DateLocal => Date.ToLocalTime();

        // TODO: Change the path when publish in domain
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? null
            : $"https://prensaestudiantil.org{ImageUrl.Substring(1)}";

        [Display(Name = "Usuario")]
        public User User { get; set; }

        //TODO modify and fix the last update date
        //[Display(Name = "Modified")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        //public DateTime LastUpdateLocal => LastUpdate ?? LastUpdate.Value.ToLocalTime();

        // Foreing keys
        public PublicationCategory PublicationCategory { get; set; }

        public ICollection<PublicationImage> PublicationImages { get; set; }
    }
}
