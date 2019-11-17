using Microsoft.AspNetCore.Identity;
using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserHelper(
            DataContext dataContext,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _context = dataContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            //await ModelAddUserAsync(user);
            return await _userManager.CreateAsync(user, password);
        }

        public async Task ModelAddUserAsync(User user)
        {
            var userTemp = _context.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            if (userTemp == null)
            {
                _context.Users.Add(user);

                await _context.SaveChangesAsync();
            }
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
    }
}
