using prensaestudiantil.Common.Models;
using prensaestudiantil.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prensaestudiantil.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private bool _isEnabled;
        private bool _isRunning;
        private string _password;
        private DelegateCommand _loginCommand;

        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            Title = "Login";
            IsEnabled = true;

            // TODO: delete this lines
            Email = "sevann.radhak@gmail.com";
            Password = "123456";
        }

        public string Email { get; set; }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter an Email and the Password", "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Check the Internet connection.", "Accept");
                return;
            }

            var request = new TokenRequest
            {
                Password = Password,
                UserName = Email
            };

            var responseToken = await _apiService.GetTokenAsync(url, "/Account", "/CreateToken", request);

            if (!responseToken.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Email or password incorrect",
                    "Accept"
                    );

                IsRunning = false;
                IsEnabled = true;
                Password = string.Empty;
                return;
            }

            var token = responseToken.Result;
            var responseUser = await _apiService.GetUserByEmailAsync(
                url,
                "/api",
                "/Users/GetUserByEmail",
                "bearer",
                token.Token,
                Email);

            if (!responseUser.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "This user has a big problem. Call support",
                    "Accept"
                    );

                IsRunning = false;
                IsEnabled = true;
                Password = string.Empty;
                return;
            }

            var parameters = new NavigationParameters
            {
                { "User", responseUser.Result}
            };

            IsRunning = false;
            IsEnabled = true;
            Password = string.Empty;

            await _navigationService.NavigateAsync("MyPublicationsPage", parameters);

        }

    }
}
