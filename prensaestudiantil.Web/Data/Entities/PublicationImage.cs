using System.ComponentModel.DataAnnotations;

namespace prensaestudiantil.Web.Data.Entities
{
    public class PublicationImage
    {
        public int Id { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        // TODO: Change the path when publish
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? $"{string.Empty}"
            //: $"https://localhost:44348{ImageUrl.Substring(1)}";
            : $"https://prensaestudiantil.org{ImageUrl.Substring(1)}";

        public Publication Publication { get; set; }
    }
}
