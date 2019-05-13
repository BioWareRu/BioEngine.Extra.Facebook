using BioEngine.Core.Properties;

namespace BioEngine.Extra.Facebook
{
    [PropertiesSet("Публикация в Facebook", Quantity = PropertiesQuantity.OnePerSite)]
    public class FacebookContentPropertiesSet : PropertiesSet
    {
        [PropertiesElement("ID поста")]
        public string? PostId { get; set; }
    }
}
