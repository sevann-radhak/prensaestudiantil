using Newtonsoft.Json;
using prensaestudiantil.Common.Helpers;
using prensaestudiantil.Common.Models;
using prensaestudiantil.Common.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace prensaestudiantil.Prism.ViewModels
{
    public class AddEditPublicationPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DateTime _date;
        private ImageSource _imageSource;
        private PublicationResponse _publication;
        private UserResponse _user;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEdit;
        private DelegateCommand _editPublicationCommand;
        private ObservableCollection<PublicationCategoryResponse> _publicationCategories;
        private PublicationCategoryResponse _publicationCategory;

        //private UserResponse _user;
        //private DelegateCommand _saveCommand;

        public AddEditPublicationPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            IsEnabled = true;
        }



        public DelegateCommand EitPublicationCommand => _editPublicationCommand ?? (_editPublicationCommand = new DelegateCommand(EitPublicationAsync));
        
        public ObservableCollection<PublicationCategoryResponse> PublicationCategories
        {
            get => _publicationCategories;
            set => SetProperty(ref _publicationCategories, value);
        }

        public PublicationCategoryResponse PublicationCategory
        {
            get => _publicationCategory;
            set => SetProperty(ref _publicationCategory, value);
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Header { get; set; }

        public string Body { get; set; }

        public string Footer { get; set; }

        public string ImageUrl { get; set; }

        public string ImageDescription { get; set; }

        public string Author { get; set; }

        public string UserId { get; set; }

        public int PublicationCategoryId { get; set; }

        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

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

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public PublicationResponse Publication
        {
            get => _publication;
            set => SetProperty(ref _publication, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        //public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("publication"))
            {
                Publication = parameters.GetValue<PublicationResponse>("publication");
                ImageSource = !string.IsNullOrEmpty(Publication.ImageUrl) ? Publication.ImageUrl : "noPublicationImage";
                IsEdit = true;
                Title = "Edit Publication";
            }
            else
            {
                IsEdit = false;
                Publication = new PublicationResponse();
                Title = "Add Publication";
                ImageSource = "noPublicationImage";
            }

            LoadPublicationCategoriesAsync();
        }
        
        private void EitPublicationAsync()
        {
            throw new NotImplementedException();
        }
        private async void LoadPublicationCategoriesAsync()
        {
            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.GetListAsync<PublicationCategoryResponse>(url, "/api", "/PublicationCategories", "bearer", token.Token);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var publicationCategories = (List<PublicationCategoryResponse>)response.Result;
            PublicationCategories = new ObservableCollection<PublicationCategoryResponse>(publicationCategories);

            if (!string.IsNullOrEmpty(Publication.PublicationCategory))
            {
                PublicationCategory = PublicationCategories.FirstOrDefault(pt => pt.Name == Publication.PublicationCategory);
            }
        }
        
        private async void SaveAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            //IsRunning = true;
            //IsEnabled = false;

            //PublicationRequest request = new PublicationRequest
            //{
            //    Author = Author,
            //    Body = Body,
            //    Date = DateTime.Now.ToUniversalTime(),
            //    Footer = Footer,
            //    Header = Header,
            //    Id = _isNew ? 0 : Id,
            //    ImageDescription = ImageDescription,
            //    //ImageUrl = null,
            //    PublicationCategoryId = PublicationCategoryId,
            //    Title = Title,
            //    UserId = User.Id
            //};

            //TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            //string url = App.Current.Resources["UrlAPI"].ToString();
            //Response<object> response = await _apiService.PutAsync(
            //    url,
            //    "/api",
            //    "/Publications",
            //    request,
            //    "bearer",
            //    token.Token);

            //IsRunning = false;
            //IsEnabled = true;

            //if (!response.IsSuccess)
            //{
            //    await App.Current.MainPage.DisplayAlert(
            //        "Error",
            //        response.Message,
            //        "Accept");
            //    return;
            //}

            //await App.Current.MainPage.DisplayAlert(
            //    "Ok",
            //    "Publication updated sucessfully.",
            //    "Accept");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Title))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must to enter a Title.",
                    "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(Header))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must to enter a Header.",
                    "Accept");
                return false;
            }

            return true;
        }
    }
}
