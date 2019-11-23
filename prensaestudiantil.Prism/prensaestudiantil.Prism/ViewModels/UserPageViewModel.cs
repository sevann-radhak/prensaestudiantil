using prensaestudiantil.Common.Models;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;

namespace prensaestudiantil.Prism.ViewModels
{
    public class UserPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private UserResponse _user;
        private ObservableCollection<PublicationItemViewModel> _publications;

        public UserPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            Title = "User";
            _navigationService = navigationService;
        }
        public ObservableCollection<PublicationItemViewModel> Publications
        {
            get => _publications;
            set => SetProperty(ref _publications, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("user"))
            {
                _user = parameters.GetValue<UserResponse>("user");
                Title = _user.FullName;
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