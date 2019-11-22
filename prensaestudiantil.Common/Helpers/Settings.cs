using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace prensaestudiantil.Common.Helpers
{
    public static class Settings
    {
        private const string _token = "token";
        private const string _user = "user";
        private static readonly string _stringDefault = string.Empty;

        private static ISettings AppSettings => CrossSettings.Current;

        public static string Token
        {
            get => AppSettings.GetValueOrDefault(_token, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_token, value);
        }

        public static string User
        {
            get => AppSettings.GetValueOrDefault(_user, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_user, value);
        }
    }
}
