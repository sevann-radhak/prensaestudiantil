using Microsoft.AspNetCore.Identity;
using prensaestudiantil.Web.Data.Entities;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Helpers
{
    public interface IUserHelper
    {
        Task<IdentityResult> AddUserAsync(User user, string password);

        Task AddUserToRoleAsync(User user, string roleName);

        Task CheckRoleAsync(string roleName);

        Task<User> GetUserByEmailAsync(string email);

        Task<bool> IsUserInRoleAsync(User user, string roleName);
    }
}
