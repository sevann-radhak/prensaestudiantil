using System;
using System.Collections.Generic;
using System.Text;

namespace prensaestudiantil.Common.Models
{
    public class UserResponse
    {
        // TODO: verify if it can be changed for Id
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        // TODO: Change the path when publish
        //public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
        //     ? $"https://TBD.azurewebsites.net/images/Users/noImage.png"
        //     : $"https://TBD.azurewebsites.net{ImageUrl.Substring(1)}";

        public ICollection<string> Roles { get; set; }

        public bool IsManager { get; set; }

        public ICollection<PublicationResponse> Publications { get; set; }

        public ICollection<YoutubeVideoResponse> YoutubeVideos { get; set; }
    }
}
