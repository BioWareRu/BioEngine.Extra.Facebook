using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Core.Social;

namespace BioEngine.Extra.Facebook.Entities
{
    public class FacebookPublishRecord : BasePublishRecord
    {
        [NotMapped] public override string Title { get; set; }
        [NotMapped] public override string Url { get; set; }
        [Required] public string PostId { get; set; }
    }
}
