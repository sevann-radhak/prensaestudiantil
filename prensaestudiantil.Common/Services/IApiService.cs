using System.Threading.Tasks;
using prensaestudiantil.Common.Models;

namespace prensaestudiantil.Common.Services
{
    public interface IApiService
    {
        Task<Response<TokenResponse>> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);
        Task<Response<UserResponse>> GetUserByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, string email);
    }
}