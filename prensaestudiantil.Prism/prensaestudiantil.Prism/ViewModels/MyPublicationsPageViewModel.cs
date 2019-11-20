using prensaestudiantil.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace prensaestudiantil.Prism.ViewModels
{
    public class MyPublicationsPageViewModel : ViewModelBase
    {
        private UserResponse _user;
        private ObservableCollection<PublicationResponse> _publications;

        public MyPublicationsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "My Publications";
        }

        public ObservableCollection<PublicationResponse> Publications
        {
            get => _publications;
            set => SetProperty(ref _publications, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("User"))
            {
                _user = parameters.GetValue<UserResponse>("User");
                Title = $"Publications of {_user.FullName }";
                Publications = new ObservableCollection<PublicationResponse>(_user.Publications);
            }
        }
    }
}