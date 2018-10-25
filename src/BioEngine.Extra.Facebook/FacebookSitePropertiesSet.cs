using System;
using BioEngine.Core.Properties;

namespace BioEngine.Extra.Facebook
{
    [PropertiesSet(Name = "Публикации в Facebook", IsEditable = true)]
    public class FacebookSitePropertiesSet : PropertiesSet
    {
        [PropertiesElement(Name = "Включено?", Type = PropertyElementType.Checkbox)]
        public bool IsEnabled { get; set; }

        [PropertiesElement(Name = "ID страницы", Type = PropertyElementType.String)]
        public string PageId { get; set; }

        [PropertiesElement(Name = "Токен", Type = PropertyElementType.PasswordString)]
        public string AccessToken { get; set; }

        [PropertiesElement(Name = "Адрес API", Type = PropertyElementType.Url)]
        public Uri ApiUrl { get; set; }
    }
}