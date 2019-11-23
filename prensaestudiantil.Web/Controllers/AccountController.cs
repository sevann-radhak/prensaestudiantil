using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Data.Entities;
using prensaestudiantil.Web.Helpers;
using prensaestudiantil.Web.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Controllers
{
    // TODO: complete the actios (update, delete) users
    // TODO: actions for edit users
    // TODO: create user with password or send email with token?
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IImageHelper _imageHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IUserHelper _userHelper;

        public AccountController(
            DataContext dataContext,
            IConfiguration configuration,
            IImageHelper imageHelper,
            IMailHelper mailHelper,
            IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _imageHelper = imageHelper;
            _mailHelper = mailHelper;
            _userHelper = userHelper;
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var users = _dataContext.Users.Include(u => u.Publications);

            //TODO: fix user roles, try select
            // Verify  if user is manager (admin)
            foreach (var user in users)
            {
                // TODO: change hardcode roleName
                user.IsManager = await _userHelper.IsInRoleAsync(user.Email, "Manager");
            };

            return View(users);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = "Password updated succssesfully";
                        return RedirectToAction(nameof(Details), new { id = user.Id });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            user.IsEnabled = true;
            await _userHelper.UpdateUserAsync(user);

            return View();
        }


        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await AddUser(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(model);
                }

                var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                var tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                _mailHelper.SendMail(model.Username, "Prensa Estudiantil - Email confirmation", $"<h1>Prensa Estudiantil - Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                TempData["Success"] = "User created successfully! The instructions to allow new user has been sent to email.";

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMonths(4),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (!await _userHelper.DeleteUserAsync(await _userHelper.GetUserByIdAsync(id)))
            {
                TempData["Error"] = "Can not delete this User";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "User deleted succssesfully";
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var currentUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (!(currentUser.Id == id || await _userHelper.IsInRoleAsync(currentUser.Email, "Manager")))
            {
                return BadRequest();
            }


            var user = await _dataContext.Users
                .Include(p => p.Publications)
                .ThenInclude(p => p.PublicationCategory)
                .Include(p => p.Publications)
                .ThenInclude(p => p.PublicationImages)
                .Include(u => u.YoutubeVideos)
                .FirstOrDefaultAsync(u => u.Id == id);

            var prueba = _userHelper.GetUserByEmailAsync(user.UserName).Result;
            if (user == null)
            {
                return NotFound();
            }

            user.Roles = await _userHelper.GetRolesAsync(user.Email);

            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var loggedUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (loggedUser != user && !await _userHelper.IsUserInRoleAsync(loggedUser, "Manager"))
            {
                TempData["Error"] = "You can not perform this action.";
                return RedirectToAction(nameof(Index));

            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ImageUrl = user.ImageUrl,
                IsEnabled = user.IsEnabled,
                IsManager = await _userHelper.IsUserInRoleAsync(user, "Manager"),
                Roles = await _userHelper.GetRolesAsync(user.Email),
                UserName = user.UserName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByIdAsync(model.Id);
                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                var loggedUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (loggedUser == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                var path = model.ImageUrl;
                user.FirstName = model.FirstName;
                user.ImageUrl = model.ImageFile != null ? await _imageHelper.UploadImageAsync(model.ImageFile) : path;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                if (user.UserName != "sevann.radhak@gmail.com" && user.UserName != "prensaestudiantil@hotmail.com")
                {
                    user.IsEnabled = model.IsEnabled;
                }
                // TODO: verify if logout external user disabled
                try
                {
                    await _userHelper.UpdateUserAsync(user);
                    if (user.UserName != "sevann.radhak@gmail.com" && user.UserName != "prensaestudiantil@hotmail.com")
                    {
                        if (model.IsManager != await _userHelper.IsUserInRoleAsync(model, "Manager"))
                        {
                            await AddOrRemoveFromRoleAsync(user, "Manager");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(model);
                }

                TempData["Success"] = "User updated successfully!";
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }

            return View(model);
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);
                }
                if (!user.IsEnabled)
                {
                    TempData["Error"] = "User disabled.";
                    return View();
                }
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Email or Password incorrect :(");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Details), new { id = user.Id });
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                var link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.SendMail(model.Email, "Prensa Estudiantil - Password Reset", $"<h1>Prensa Estudiantill Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");

                TempData["Success"] = "The instructions to recover your password has been sent to email.";
                return View();
            }

            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                if (_userHelper.ResetPasswordAsync(user, model.Token, model.Password).Result.Succeeded)
                {
                    await _userHelper.LoginAsync(new LoginViewModel { Password = model.Password, RememberMe = false, Username = model.UserName });
                    TempData["Success"] = "Password reset successfully!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Error while resetting the password. Try again or call support";
                return View(model);
            }

            TempData["Error"] = "User not found.";
            return View(model);
        }

        private async Task AddOrRemoveFromRoleAsync(User user, string roleName)
        {
            if (await _userHelper.IsUserInRoleAsync(user, roleName))
            {
                await _userHelper.RemoveUserFromRole(user, roleName);
            }
            else
            {
                await _userHelper.AddUserToRoleAsync(user, roleName);
            }
        }

        [Authorize(Roles = "Manager")]
        private async Task<User> AddUser(AddUserViewModel model)
        {
            var user = new User
            {
                Email = model.Username,
                FirstName = model.FirstName,
                IsEnabled = false,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username
            };

            user.ImageUrl = model.ImageFile != null ? await _imageHelper.UploadImageAsync(model.ImageFile) : null;

            var result = await _userHelper.AddUserAsync(user, model.UserName);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            var newUser = await _userHelper.GetUserByEmailAsync(model.Username);
            //TODO: modify harcode
            await _userHelper.AddUserToRoleAsync(newUser, "Writer");

            return newUser;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dataContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}