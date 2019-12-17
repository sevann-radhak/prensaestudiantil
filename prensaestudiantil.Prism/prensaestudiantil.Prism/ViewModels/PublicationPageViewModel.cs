using prensaestudiantil.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prensaestudiantil.Prism.ViewModels
{
    public class PublicationPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private PublicationResponse _publication;
        private DelegateCommand _editPublicationCommand;

        public PublicationPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Publication";
            _navigationService = navigationService;
        }

        public DelegateCommand EditPublicationCommand => _editPublicationCommand ?? (_editPublicationCommand = new DelegateCommand(EditPublicationAsync));

        public PublicationResponse Publication
        {
            get => _publication;
            set => SetProperty(ref _publication, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("publication"))
            {
                Publication = parameters.GetValue<PublicationResponse>("publication");
                Title = Publication.Title;
            }
        }

        private async void EditPublicationAsync()
        {
            var parameters = new NavigationParameters { { "publication", Publication } };
            await _navigationService.NavigateAsync("AddEditPublicationPage", parameters);
        }
    }
}
