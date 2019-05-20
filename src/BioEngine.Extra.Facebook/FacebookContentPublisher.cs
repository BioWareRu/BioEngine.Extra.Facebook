using System;
using System.Threading.Tasks;
using BioEngine.Core.DB;
using BioEngine.Core.Entities;
using BioEngine.Core.Publishers;
using BioEngine.Extra.Facebook.Entities;
using BioEngine.Extra.Facebook.Service;
using Microsoft.Extensions.Logging;

namespace BioEngine.Extra.Facebook
{
    public class FacebookContentPublisher : BaseContentPublisher<FacebookConfig, FacebookPublishRecord>
    {
        private readonly FacebookService _facebookService;

        public FacebookContentPublisher(FacebookService facebookService, BioContext dbContext,
            ILogger<IContentPublisher<FacebookConfig>> logger) : base(dbContext, logger)
        {
            _facebookService = facebookService;
        }

        protected override async Task<FacebookPublishRecord> DoPublishAsync(IContentEntity entity, Site site,
            FacebookConfig config)
        {
            var postId = await _facebookService.PostLinkAsync(new Uri($"{site.Url}{entity.PublicUrl}"),
                config);
            if (string.IsNullOrEmpty(postId))
            {
                throw new Exception($"Can't create facebook post for item {entity.Title} ({entity.Id.ToString()})");
            }

            var record = new FacebookPublishRecord
            {
                ContentId = entity.Id, Type = entity.GetType().FullName, PostId = postId, SiteIds = new[] {site.Id}
            };

            return record;
        }

        protected override async Task<bool> DoDeleteAsync(FacebookPublishRecord record, FacebookConfig config)
        {
            var deleted = await _facebookService.DeletePostAsync(record.PostId, config);
            if (deleted)
            {
                return true;
            }

            throw new Exception("Can't delete content post from Facebook");
        }
    }
}
