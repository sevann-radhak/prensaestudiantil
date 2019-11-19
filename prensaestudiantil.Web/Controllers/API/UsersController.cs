using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prensaestudiantil.Common.Models;
using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Data.Entities;
using prensaestudiantil.Web.Helpers;

namespace prensaestudiantil.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public UsersController(
            DataContext dataContext,
            IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Login failed."
                });
            }

            User user = await _dataContext.Users
                .Include(u => u.YoutubeVideos)
                .Include(u => u.Publications)
                .ThenInclude(p => p.PublicationCategory)
                .Include(u => u.Publications)
                .ThenInclude(p => p.PublicationImages)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

            if (user == null)
            {
                return NotFound(new Response<object>
                {
                    IsSuccess = false,
                    Message = "User not found."
                });
            }

            UserResponse response = new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                ImageUrl = user.ImageFullPath,
                LastName = user.LastName,
                IsManager = await _userHelper.IsInRoleAsync(user.Email, "Manager"),
                Publications = user.Publications?.Select(p => new PublicationResponse
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
                    PublicationImages = p.PublicationImages?.Select(pi => new PublicationImageResponse
                    {
                        Description = pi.Description,
                        Id = pi.Id,
                        ImageUrl = pi.ImageFullPath
                    }).ToList(),
                    Title = p.Title,
                    User = p.User.FullName
                }).ToList(),
                Roles = await _userHelper.GetRolesAsync(user.Email),
                YoutubeVideos = user.YoutubeVideos?.Select(y => new YoutubeVideoResponse
                {
                    Name = y.Name,
                    URL = y.URL
                }).ToList()
            };

            return Ok(new Response<UserResponse>
            {
                IsSuccess = true,
                Result = response
            });
        }
    }
}
