using prensaestudiantil.Common.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prensaestudiantil.Prism.ViewModels
{
    public class RegisterUserPageViewModel : ViewModelBase
    {
        //private string _email;
        //private string _firstName;
        private bool _isEnabled;
        private bool _isRunning;
        //private string _lastName;
        //private string _phone;
        private DelegateCommand _registerUserCommand;

        public RegisterUserPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Register new User";
            IsEnabled = true;
        }

        public DelegateCommand RegisterUserCommand => _registerUserCommand ?? (_registerUserCommand = new DelegateCommand(RegisterUser));

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

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

        private async void RegisterUser()
        {
            var isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }
        }

        private async Task<bool> ValidateData()
        {           
            if (string.IsNullOrEmpty(FirstName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a Firstname.", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(LastName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a Lastname.", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(Email) || !RegexHelper.IsValidEmail(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a valid Email Address.", "Accept");
                return false;
            }

            return true;
        }
    }
}
