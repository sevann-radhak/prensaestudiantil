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
    public class PublicationsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private UserResponse _user;
        private ObservableCollection<PublicationItemViewModel> _publications;

        public PublicationsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "My Publications";
        }

        public ObservableCollection<PublicationItemViewModel> Publications
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
                Publications = new ObservableCollection<PublicationItemViewModel>(
                    _user.Publications.Select(p => new PublicationItemViewModel(_navigationService) 
                    {
                        Author = p.Author,
                        Body = p.Body,
                        Date = p.Date,
                        Footer = p.Footer,
                        Header = p.Header,
                        Id = p.Id,
                        ImageDescription = p.ImageDescription,
                        ImageUrl = p.ImageUrl,
                        LastUpdate = p.LastUpdate,
                        PublicationCategory = p.PublicationCategory,
                        PublicationImages = p.PublicationImages,
                        Title = p.Title,
                        User = p.User
                    }
                    ).ToList());
            }
        }
    }
}