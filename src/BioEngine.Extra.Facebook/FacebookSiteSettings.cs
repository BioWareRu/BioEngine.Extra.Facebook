using System;
using BioEngine.Core.Providers;

namespace BioEngine.Extra.Facebook
{
    [SettingsClass(Name = "Публикации в Facebook", IsEditable = true)]
    public class FacebookSiteSettings : SettingsBase
    {
        [SettingsProperty(Name = "Включено?", Type = SettingType.Checkbox)]
        public bool IsEnabled { get; set; }

        [SettingsProperty(Name = "ID страницы", Type = SettingType.String)]
        public string PageId { get; set; }

        [SettingsProperty(Name = "Токен", Type = SettingType.PasswordString)]
        public string AccessToken { get; set; }

        [SettingsProperty(Name = "Адрес API", Type = SettingType.Url)]
        public Uri ApiUrl { get; set; }
    }
}