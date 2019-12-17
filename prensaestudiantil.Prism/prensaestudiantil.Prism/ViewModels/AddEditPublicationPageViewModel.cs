using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using prensaestudiantil.Common.Helpers;
using prensaestudiantil.Common.Models;
using prensaestudiantil.Common.Services;
using prensaestudiantil.Prism.Helpers;
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
        private DelegateCommand _editPublicationCommand;
        private MediaFile _file;
        private DelegateCommand _changeImageCommand;

        private ImageSource _imageSource;
        private bool _isEdit;
        private bool _isEnabled;
        private bool _isRunning;
        private PublicationResponse _publication;
        private ObservableCollection<PublicationCategoryResponse> _publicationCategories;
        private PublicationCategoryResponse _publicationCategory;
        private DelegateCommand _saveCommand;
        private UserResponse _user;

        public AddEditPublicationPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            IsEnabled = true;
        }

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public DelegateCommand EitPublicationCommand => _editPublicationCommand ?? (_editPublicationCommand = new DelegateCommand(ChangeImageAsync));
        
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

        //public int Id { get; set; }

        //public string Title { get; set; }

        //public string Header { get; set; }

        //public string Body { get; set; }

        //public string Footer { get; set; }

        //public string ImageUrl { get; set; }

        //public string ImageDescription { get; set; }

        //public string Author { get; set; }

        //public string UserId { get; set; }

        //public int PublicationCategoryId { get; set; }

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

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

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

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Where do you want to get the picture?",
                "Cancel",
                null,
                "From gallery",
                "From camera");

            if (source == "Cancel")
            {
                _file = null;
                return;
            }

            if (source == "From camera")
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = _file.GetStream();
                    return stream;
                });
            }
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

            IsRunning = true;
            IsEnabled = false;

            PublicationRequest request = new PublicationRequest
            {
                Author = Publication.Author,
                Body = Publication.Body,
                Date = DateTime.Now.ToUniversalTime(),
                Footer = Publication.Footer,
                Header = Publication.Header,
                Id = IsEdit ? Publication.Id : 0,
                ImageDescription = Publication.ImageDescription,
                //ImageUrl = null,
                ImageArray = _file != null ? FilesHelper.ReadFully(_file.GetStream()) : null,
                PublicationCategoryId = PublicationCategory.Id,
                Title = Publication.Title,
                UserId = User.Id
            };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            string url = App.Current.Resources["UrlAPI"].ToString();
            
            Response<object> response;
            if (IsEdit)
            {
                response = await _apiService.PutAsync(
                url,
                "/api",
                "/Publications",
                request,
                "bearer",
                token.Token);
            }
            else
            {
                response = await _apiService.PostAsync(
                url,
                "/api",
                "/Publications",
                request,
                "bearer",
                token.Token);
            }                

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            await App.Current.MainPage.DisplayAlert(
                "Ok",
                "Info saved sucessfully.",
                "Accept");

            await PublicationsPageViewModel.GetInstance().UpdatePublications();

            await _navigationService.NavigateAsync("PublicationsPage");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Publication.Title))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must to enter a Title.",
                    "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(Publication.Header))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must to enter a Header.",
                    "Accept");
                return false;
            }

            if (PublicationCategory == null)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must to select a Category.",
                    "Accept");
                return false;
            }

            return true;
        }
    }
}
