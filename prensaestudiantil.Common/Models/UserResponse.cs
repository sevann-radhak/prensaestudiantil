using System;
using System.Collections.Generic;
using System.Text;

namespace prensaestudiantil.Common.Models
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string PhoneNumber { get; set; }

        public ICollection<string> Roles { get; set; }

        public bool IsManager { get; set; }

        public ICollection<PublicationResponse> Publications { get; set; }

        public ICollection<YoutubeVideoResponse> YoutubeVideos { get; set; }
    }
}
