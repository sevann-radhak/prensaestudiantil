using prensaestudiantil.Web.Data.Entities;
using System.Collections.Generic;

namespace prensaestudiantil.Web.Models
{
    public class MainIndexViewModel
    {
        public ICollection<Publication> OpinionPublications { get; set; }

        public ICollection<Publication> Publications { get; set; }

        public ICollection<PublicationImage> PublicationImages { get; set; }

        public ICollection<YoutubeVideo> YoutubeVideos { get; set; }
    }
}
