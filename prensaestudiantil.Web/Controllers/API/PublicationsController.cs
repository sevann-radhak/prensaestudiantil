using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prensaestudiantil.Common.Models;
using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PublicationsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public PublicationsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> GetAllAsync()
        {

            var publications = _dataContext.Publications
                .Include(p => p.PublicationImages)
                .Include(p => p.PublicationCategory)
                .Take(30)
                .OrderByDescending(p => p.DateLocal);

            return Ok(new PublicationsResponse 
            {
                Publications = await publications.Select(p => new PublicationResponse
                {
                    Author = p.Author,
                    Body = p.Body,
                    Date = p.Date,
                    Footer = p.Footer,
                    Header = p.Header,
                    Id = p.Id,
                    ImageDescription = p.ImageDescription,
                    ImageUrl = p.ImageFullPath,
                    LastUpdate = p.LastUpdate,
                    PublicationCategory = p.PublicationCategory.Name,
                    PublicationImages = p.PublicationImages.Select(pi => new PublicationImageResponse
                    {
                        Description = pi.Description,
                        Id = pi.Id,
                        ImageUrl = pi.ImageFullPath
                    }).ToList(),
                    Title = p.Title,
                    User = p.User.FullName
                }).OrderByDescending(p => p.Date)
            .ToListAsync()
            });
        }
    }
}
