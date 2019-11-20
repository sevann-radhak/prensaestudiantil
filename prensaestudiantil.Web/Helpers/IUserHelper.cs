using Microsoft.AspNetCore.Identity;
using prensaestudiantil.Web.Data.Entities;
using prensaestudiantil.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Helpers
{
    public interface IUserHelper
    {
        Task<IdentityResult> AddUserAsync(User user, string password);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task CheckRoleAsync(string roleName); 
        
        Task<bool> DeleteUserAsync(User user);

        Task<IList<string>> GetRolesAsync(string email);

        Task<User> GetUserByEmailAsync(string email);
        
        Task<User> GetUserByIdAsync(string id);

        Task<bool> IsInRoleAsync(string email, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<SignInResult> ValidatePasswordAsync(User user, string password);

    }
}
