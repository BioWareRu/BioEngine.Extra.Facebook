using BioEngine.Core.Properties;

namespace BioEngine.Extra.Facebook
{
    [PropertiesSet(Name = "Публикация в Facebook", Quantity = PropertiesQuantity.OnePerSite)]
    public class FacebookContentPropertiesSet : PropertiesSet
    {
        [PropertiesElement(Name = "ID поста", Type = PropertyElementType.String)]
        public string PostId { get; set; }
    }
}