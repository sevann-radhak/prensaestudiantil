using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Data.Entities
{
    public class PublicationCategory
    {
        public int Id { get; set; }

        [Display(Name = "Category")]
        public string Name { get; set; }

        // Foreing key
        public ICollection<Publication> Publications { get; set; }
    }
}
