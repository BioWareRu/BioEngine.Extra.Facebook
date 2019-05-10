using System;
using BioEngine.Core.Properties;

namespace BioEngine.Extra.Facebook
{
    [PropertiesSet("Публикации в Facebook", IsEditable = true)]
    public class FacebookSitePropertiesSet : PropertiesSet
    {
        [PropertiesElement("Включено?", PropertyElementType.Checkbox)]
        public bool IsEnabled { get; set; }

        [PropertiesElement("ID страницы")] public string PageId { get; set; }

        [PropertiesElement("Токен", PropertyElementType.PasswordString)]
        public string AccessToken { get; set; }

        [PropertiesElement("Адрес API", PropertyElementType.Url)]
        public Uri ApiUrl { get; set; }
    }
}
