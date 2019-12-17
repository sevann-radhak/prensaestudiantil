using System;
using prensaestudiantil.Common.Models;
using Prism.Commands;
using Prism.Navigation;

namespace prensaestudiantil.Prism.ViewModels
{
    public class PublicationItemViewModel : PublicationResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectPublicationCommand;

        public PublicationItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectPublicationCommand => _selectPublicationCommand ?? (_selectPublicationCommand = new DelegateCommand(SelectPublicationAsync));

        private async void SelectPublicationAsync()
        {
            var parameters = new NavigationParameters
            {
                { "publication", this }
            };
            await _navigationService.NavigateAsync("PublicationPage", parameters);
        }
    }
}
