using System.ComponentModel.DataAnnotations;

namespace prensaestudiantil.Web.Data.Entities
{
    public class PublicationImage
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        // TODO: Change the path when publish
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? $"{string.Empty}"
            : $"https://prensaestudiantil.azurewebsites.net{ImageUrl.Substring(1)}";

        // Foreing key
        public Publication Publication { get; set; }
    }
}
