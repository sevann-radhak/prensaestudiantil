using Newtonsoft.Json;
using prensaestudiantil.Common.Helpers;
using prensaestudiantil.Common.Models;
using prensaestudiantil.Common.Services;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace prensaestudiantil.Prism.ViewModels
{
    public class PublicationsPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _addEditPublicationCommand;
        private ObservableCollection<PublicationItemViewModel> _publications;
        private static PublicationsPageViewModel _instance;

        public PublicationsPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            _apiService = apiService;
            LoadPublications();
            Title = "Publications";
        }
        public static PublicationsPageViewModel GetInstance()
        {
            return _instance;
        }
        public async Task UpdatePublications()
        {
            LoadPublications();
        }


        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public DelegateCommand AddEditPublicationCommand => _addEditPublicationCommand ?? (_addEditPublicationCommand = new DelegateCommand(AddEditPublicationAsync));


        public ObservableCollection<PublicationItemViewModel> Publications
        {
            get => _publications;
            set => SetProperty(ref _publications, value);
        }

        private async void AddEditPublicationAsync()
        {
            await _navigationService.NavigateAsync("AddEditPublicationPage");
        }

        private async void LoadPublications()
        {
            IsRunning = true;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response<PublicationsResponse> response = await _apiService.GetPublicationsAsync(
                url,
                "/api",
                "/Publications",
                "bearer",
                token.Token);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            Publications = new ObservableCollection<PublicationItemViewModel>(
                response.Result.Publications?.Select(p => new PublicationItemViewModel(_navigationService)
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