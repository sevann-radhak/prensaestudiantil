using Prism;
using Prism.Ioc;
using prensaestudiantil.Prism.ViewModels;
using prensaestudiantil.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using prensaestudiantil.Common.Services;
using Newtonsoft.Json;
using prensaestudiantil.Common.Models;
using prensaestudiantil.Common.Helpers;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace prensaestudiantil.Prism
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            if (Settings.IsRemembered && token?.Expiration > DateTime.Now)
            {
                await NavigationService.NavigateAsync("/PrensaMasterDetailPage/NavigationPage/PublicationsPage");
            }
            else
            {
                await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
            }

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<PublicationsPage, PublicationsPageViewModel>();
            containerRegistry.RegisterForNavigation<PublicationPage, PublicationPageViewModel>();
            containerRegistry.RegisterForNavigation<PrensaMasterDetailPage, PrensaMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterUserPage, RegisterUserPageViewModel>();
            containerRegistry.RegisterForNavigation<UserPage, UserPageViewModel>();
            containerRegistry.RegisterForNavigation<RememberPasswordPage, RememberPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPublicationPage, AddEditPublicationPageViewModel>();
        }
    }
}
