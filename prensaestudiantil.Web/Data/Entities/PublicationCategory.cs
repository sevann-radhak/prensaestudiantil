using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prensaestudiantil.Web.Data.Entities
{
    public class PublicationCategory : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Name { get; set; }

        public ICollection<Publication> Publications { get; set; }
    }
}
