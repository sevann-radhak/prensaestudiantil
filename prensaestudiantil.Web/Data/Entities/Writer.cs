using System.Collections.Generic;

namespace prensaestudiantil.Web.Data.Entities
{
    public class Writer
    {
        public int Id { get; set; }

        public User User { get; set; }

        // Foreing keys
        //public ICollection<Publication> Publications { get; set; }

        //public ICollection<YoutubeVideo> YoutubeVideos { get; set; }
    }
}
