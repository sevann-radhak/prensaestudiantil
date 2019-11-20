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
        private PublicationResponse _publication;

        public PublicationPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Publication";
        }

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
    }
}
