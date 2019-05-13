using System;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Core.DB;
using BioEngine.Core.Entities;
using BioEngine.Core.Properties;
using BioEngine.Core.Repository;
using BioEngine.Extra.Facebook.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Extra.Facebook
{
    public class FacebookContentHook : BaseRepositoryHook
    {
        private readonly PropertiesProvider _propertiesProvider;
        private readonly FacebookService _facebookService;
        private readonly BioContext _bioContext;
        private readonly ILogger<FacebookContentHook> _logger;

        public FacebookContentHook(PropertiesProvider propertiesProvider, FacebookService facebookService,
            BioContext bioContext, ILogger<FacebookContentHook> logger)
        {
            _propertiesProvider = propertiesProvider;
            _facebookService = facebookService;
            _bioContext = bioContext;
            _logger = logger;
        }

        public override bool CanProcess(Type type)
        {
            return typeof(Post).IsAssignableFrom(type);
        }

        public override async Task<bool> AfterSaveAsync<T>(T item, PropertyChange[]? changes = null,
            IBioRepositoryOperationContext? operationContext = null)
        {
            if (item is Post content)
            {
                var sites = await _bioContext.Sites.Where(s => content.SiteIds.Contains(s.Id)).ToListAsync();
                foreach (var site in sites)
                {
                    var siteProperties = await _propertiesProvider.GetAsync<FacebookSitePropertiesSet>(site);
                    if (!siteProperties.IsEnabled)
                    {
                        _logger.LogInformation("Facebook is not enabled for site {siteTitle}", site.Title);
                        continue;
                    }

                    var facebookConfig = new FacebookModuleConfig
                    {
                        AccessToken = siteProperties.AccessToken,
                        Url = siteProperties.ApiUrl != null ? new Uri(siteProperties.ApiUrl) : null,
                        PageId = siteProperties.PageId
                    };

                    var itemProperties =
                        await _propertiesProvider.GetAsync<FacebookContentPropertiesSet>(content, site.Id);

                    var hasChanges = changes != null && changes.Any(c => c.Name == nameof(content.Url));

                    if (!string.IsNullOrEmpty(itemProperties.PostId) && (hasChanges || !content.IsPublished))
                    {
                        var deleted = await _facebookService.DeletePostAsync(itemProperties.PostId, facebookConfig);
                        if (!deleted)
                        {
                            throw new Exception("Can't delete content post from Facebook");
                        }

                        itemProperties.PostId = null;
                    }

                    if (content.IsPublished && (string.IsNullOrEmpty(itemProperties.PostId) || hasChanges))
                    {
                        var postId = await _facebookService.PostLinkAsync(new Uri($"{site.Url}{content.PublicUrl}"),
                            facebookConfig);
                        if (!string.IsNullOrEmpty(postId))
                        {
                            itemProperties.PostId = postId;
                        }
                    }

                    await _propertiesProvider.SetAsync(itemProperties, content, site.Id);
                }


                return true;
            }

            return false;
        }
    }
}
