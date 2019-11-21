﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using prensaestudiantil.Web.Data.Entities;

namespace prensaestudiantil.Web.Models
{
    public class EditUserViewModel : User
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
