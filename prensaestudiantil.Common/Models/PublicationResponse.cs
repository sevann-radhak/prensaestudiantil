using System;
using System.Collections.Generic;
using System.Text;

namespace prensaestudiantil.Common.Models
{
    public class PublicationResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

         public string Header { get; set; }

        public string Body { get; set; }

        public string Footer { get; set; }

        public DateTime Date { get; set; }

        public DateTime? LastUpdate { get; set; }

        public string ImageUrl { get; set; }

        public string ImageDescription { get; set; }

        public string Author { get; set; }

       public DateTime DateLocal => Date.ToLocalTime();

        public string User { get; set; }

        public string PublicationCategory { get; set; }

        public ICollection<PublicationImageResponse> PublicationImages { get; set; }
    }
}
