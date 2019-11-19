using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prensaestudiantil.Web.Data.Entities
{
    public class Publication
    {
        public int Id { get; set; }

        [Display(Name = "Title*")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(150, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Title { get; set; }

        [Display(Name = "Header*")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DataType(DataType.MultilineText)]
        public string Header { get; set; }

        [Display(Name = "Body")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Display(Name = "Footer")]
        [MaxLength(250, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Footer { get; set; }

        [Display(Name = "Publication Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Modify Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? LastUpdate { get; set; }

        [Display(Name = "Main Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Image Description")]
        [MaxLength(150, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string ImageDescription { get; set; }

        // Only for external references for author
        [Display(Name = "Author")]
        [MaxLength(60, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Author { get; set; }

        [Display(Name = "Publication Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DateLocal => Date.ToLocalTime();

        // TODO: Change the path when publish in domain
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? $"{string.Empty}"
             : $"https://prensaestudiantil.azurewebsites.net{ImageUrl.Substring(1)}";

        [Display(Name = "User")]
        public User User { get; set; }

        //TODO modify and fix the last update date
        //[Display(Name = "Modified")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        //public DateTime LastUpdateLocal => LastUpdate == null ? null : DateTime.Now;
        //string.IsNullOrEmpty(LastUpdate.ToString())
        //? null
        //: LastUpdate.Value.ToLocalTime();

        // Foreing keys
        public PublicationCategory PublicationCategory { get; set; }

        public ICollection<PublicationImage> PublicationImages { get; set; }
    }
}
