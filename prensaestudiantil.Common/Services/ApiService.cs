using Newtonsoft.Json;
using Plugin.Connectivity;
using prensaestudiantil.Common.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace prensaestudiantil.Common.Services
{
    public class ApiService : IApiService
    {
        public async Task<Response<object>> ChangePasswordAsync(
        string urlBase,
        string servicePrefix,
        string controller,
        ChangePasswordRequest changePasswordRequest,
        string tokenType,
        string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(changePasswordRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Response<object>>(answer);

                return response.IsSuccessStatusCode ? new Response<object> { IsSuccess = true, Message = obj.Message }
                : new Response<object> { IsSuccess = false, Message = obj.Message };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<bool> CheckConnectionAsync(string url)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            return await CrossConnectivity.Current.IsRemoteReachable(url);
        }

        public async Task<Response<TokenResponse>> GetTokenAsync(
               string urlBase,
               string servicePrefix,
               string controller,
               TokenRequest request)
        {
            try
            {
                string requestString = JsonConvert.SerializeObject(request);
                StringContent content = new StringContent(requestString, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<TokenResponse>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(result);
                return new Response<TokenResponse>
                {
                    IsSuccess = true,
                    Result = token
                };
            }
            catch (Exception ex)
            {
                return new Response<TokenResponse>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<UserResponse>> GetUserByEmailAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            string email)
        {
            try
            {
                EmailRequest request = new EmailRequest { Email = email };
                string requestString = JsonConvert.SerializeObject(request);
                StringContent content = new StringContent(requestString, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<UserResponse>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                UserResponse user = JsonConvert.DeserializeObject<UserResponse>(result);
                return new Response<UserResponse>
                {
                    IsSuccess = true,
                    Result = user
                };
            }
            catch (Exception ex)
            {
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<object>> PutAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                return new Response<object>
                {
                    IsSuccess = true,
                    Result = JsonConvert.DeserializeObject<T>(answer),
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<object>> RecoverPasswordAsync(
        string urlBase,
        string servicePrefix,
        string controller,
        EmailRequest emailRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(emailRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Response<object>>(answer);
                return new Response<object>
                {
                    Message = obj.Message
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


        public async Task<Response<UserResponse>> RegisterUserAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            UserRequest userRequest)
        {
            try
            {
                string requestString = JsonConvert.SerializeObject(userRequest);
                StringContent content = new StringContent(requestString, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<UserResponse>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var userResponse = JsonConvert.DeserializeObject<UserResponse>(result);
                return new Response<UserResponse>
                {
                    IsSuccess = true,
                    Result = userResponse
                };

            }
            catch (Exception ex)
            {
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
