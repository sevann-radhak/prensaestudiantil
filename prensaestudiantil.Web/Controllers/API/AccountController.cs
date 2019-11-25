using Microsoft.AspNetCore.Mvc;
using prensaestudiantil.Common.Models;
using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Helpers;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Controllers.API
{
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;

        public AccountController(
            DataContext dataContext,
            IUserHelper userHelper,
            IMailHelper mailHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
        }


        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await _userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "This email is not assigned to any user."
                });
            }

            var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
            _mailHelper.SendMail(request.Email, "Password Reset", $"<h1>Recover Password</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

            return Ok("An email with instructions to change the password was sent.");
        }
    }
}
