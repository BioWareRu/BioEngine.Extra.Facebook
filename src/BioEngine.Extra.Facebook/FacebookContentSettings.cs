using BioEngine.Core.Providers;

namespace BioEngine.Extra.Facebook
{
    [SettingsClass(Name = "Публикация в Facebook", Mode = SettingMode.OnePerSite)]
    public class FacebookContentSettings : SettingsBase
    {
        [SettingsProperty(Name = "ID поста", Type = SettingType.String)]
        public string PostId { get; set; }
    }
}