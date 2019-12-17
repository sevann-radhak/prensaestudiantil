using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;

        public UsersController(
            DataContext dataContext,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            IMailHelper mailHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response<object>
                {
                    Message = "This email is not assigned to any user."
                });
            }

            var result = await _userHelper.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new Response<object>
                {
                    Message = result.Errors.FirstOrDefault().Description
                });
            }

            return Ok(new Response<object>
            {
                Message = "The password was changed successfully!"
            });
        }

        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmailAsync(EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                return NotFound("User not found.");
            }

            return Ok(new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
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
                }).OrderByDescending(p => p.Date)
                .ToList(),
                PhoneNumber = user.PhoneNumber,
                Roles = await _userHelper.GetRolesAsync(user.Email),
                YoutubeVideos = user.YoutubeVideos?.Select(y => new YoutubeVideoResponse
                {
                    Name = y.Name,
                    URL = y.URL
                }).ToList()
            });
        }

        [HttpPost]
        [Route("PostUser")]
        public async Task<IActionResult> PostUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userHelper.GetUserByEmailAsync(request.Email);
            if (user != null)
            {
                return BadRequest("This Email is already registered.");
            }

            user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                UserName = request.Email
            };

            var result = await _userHelper.AddUserAsync(user, user.Email);
            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }

            UserResponse userResponse = await _converterHelper.GetUSerResponseViewModelByEmail(user.Email);
            var userNew = await _userHelper.GetUserByEmailAsync(request.Email);
            await _userHelper.AddUserToRoleAsync(user, "Writer");

            var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(userNew);
            var tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            _mailHelper.SendMail(request.Email, "Prensa Estudiantil - Email confirmation", $"<h1>Prensa Estudiantil - Email Confirmation</h1>" +
                $"To allow the user, " +
                $"please click on this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

            return Ok(userResponse);
        }

        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEntity = await _userHelper.GetUserByEmailAsync(request.Email);
            if (userEntity == null)
            {
                return BadRequest("User not found.");
            }

            userEntity.FirstName = request.FirstName;
            userEntity.LastName = request.LastName;
            userEntity.PhoneNumber = request.Phone;

            var respose = await _userHelper.UpdateUserAsync(userEntity);
            if (!respose.Succeeded)
            {
                return BadRequest(respose.Errors.FirstOrDefault().Description);
            }

            var updatedUser = await _userHelper.GetUserByEmailAsync(request.Email);
            return Ok(updatedUser);
        }
    }
}
