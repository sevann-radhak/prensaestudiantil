using Newtonsoft.Json;
using prensaestudiantil.Common.Helpers;
using prensaestudiantil.Common.Models;
using prensaestudiantil.Common.Services;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;

namespace prensaestudiantil.Prism.ViewModels
{
    public class ModifyUserPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private bool _isEnabled;
        private UserResponse _user;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _changePasswordCommand;
        private DelegateCommand _registerUserCommand;
        private DelegateCommand _saveCommand;

        public ModifyUserPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            Title = "Modify User";
            _navigationService = navigationService;
            _apiService = apiService;
            IsEnabled = true;
            User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ?? (_changePasswordCommand = new DelegateCommand(ChangePasswordAsync));

        public DelegateCommand RegisterUserCommand => _registerUserCommand ?? (_registerUserCommand = new DelegateCommand(RegisterUser));

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

            public UserResponse User
            {
                get => _user;
                set => SetProperty(ref _user, value);
            }

        private async void ChangePasswordAsync()
        {
            await _navigationService.NavigateAsync("ChangePasswordPage");
        }

        private async void RegisterUser()
        {
            await _navigationService.NavigateAsync("RegisterUserPage");
        }

        private async void SaveAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            UserRequest userRequest = new UserRequest
            {
                Email = User.Email,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Phone = User.PhoneNumber
            };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response<object> response = await _apiService.PutAsync(
                url,
                "/api",
                "/Users",
                userRequest,
                "bearer",
                token.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            Settings.User = JsonConvert.SerializeObject(response.Result);

            await App.Current.MainPage.DisplayAlert(
                "Ok",
                "User updated sucessfully.",
                "Accept");

        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(User.FirstName))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must to enter a first name.",
                    "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must to enter a last name.",
                    "Accept");
                return false;
            }

            return true;
        }
    }
}
