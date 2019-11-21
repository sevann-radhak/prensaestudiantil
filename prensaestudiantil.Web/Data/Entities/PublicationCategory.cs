using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Data.Entities
{
    public class PublicationCategory : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Name { get; set; }

        // Foreing key
        public ICollection<Publication> Publications { get; set; }
    }
}
