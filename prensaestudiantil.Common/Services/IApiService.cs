using System;
using System.Threading.Tasks;
using prensaestudiantil.Common.Models;

namespace prensaestudiantil.Common.Services
{
    public interface IApiService
    {
        Task<Response<object>> ChangePasswordAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            ChangePasswordRequest changePasswordRequest,
            string tokenType,
            string accessToken);

        Task<Response<object>> GetListAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);

    Task<bool> CheckConnectionAsync(string url);

        Task<Response<PublicationsResponse>> GetPublicationsAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            //T model,
            string tokenType,
            string accessToken);

        Task<Response<TokenResponse>> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);

        Task<Response<UserResponse>> GetUserByEmailAsync(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, string email);

        Task<Response<object>> PutAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken);


        Task<Response<object>> RecoverPasswordAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            EmailRequest emailRequest);

        Task<Response<UserResponse>> RegisterUserAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            UserRequest userRequest);

    }
}