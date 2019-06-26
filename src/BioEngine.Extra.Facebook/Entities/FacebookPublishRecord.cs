using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Core.DB;
using BioEngine.Core.Social;

namespace BioEngine.Extra.Facebook.Entities
{
    [Entity("facebookpublishrecord")]
    public class FacebookPublishRecord : BasePublishRecord
    {
        [NotMapped] public override string Title { get; set; }
        [Required] public string PostId { get; set; }
    }
}
