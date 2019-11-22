using prensaestudiantil.Web.Data.Entities;
using System.Collections.Generic;

namespace prensaestudiantil.Web.Models
{
    public class MainIndexViewModel
    {
        public ICollection<PublicationImage> PublicationImages { get; set; }
    }
}
