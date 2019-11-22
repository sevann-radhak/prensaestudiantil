using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prensaestudiantil.Prism.ViewModels
{
    public class ModifyUsesrPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _registerUserCommand;

        public ModifyUsesrPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Modify User";
            _navigationService = navigationService;
        }

        public DelegateCommand RegisterUserCommand => _registerUserCommand ?? (_registerUserCommand = new DelegateCommand(RegisterUser));

        private async void RegisterUser()
        {
            await _navigationService.NavigateAsync("RegisterUserPage");
        }
    }
}
