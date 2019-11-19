using System;
using System.Collections.Generic;
using System.Text;

namespace prensaestudiantil.Common.Models
{
    public class PublicationImageResponse
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        // TODO: Change the path when publish
        //public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
        //    ? $"{string.Empty}"
        //    : $"https://TBD.azurewebsites.net{ImageUrl.Substring(1)}";
    }
}
