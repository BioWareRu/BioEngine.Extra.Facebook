using System;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Core.DB;
using BioEngine.Core.Entities;
using BioEngine.Core.Providers;
using BioEngine.Core.Repository;
using BioEngine.Extra.Facebook.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Extra.Facebook
{
    public class FacebookContentFilter : BaseRepositoryFilter
    {
        private readonly SettingsProvider _settingsProvider;
        private readonly FacebookService _facebookService;
        private readonly BioContext _bioContext;
        private readonly ILogger<FacebookContentFilter> _logger;

        public FacebookContentFilter(SettingsProvider settingsProvider, FacebookService facebookService,
            BioContext bioContext, ILogger<FacebookContentFilter> logger)
        {
            _settingsProvider = settingsProvider;
            _facebookService = facebookService;
            _bioContext = bioContext;
            _logger = logger;
        }

        public override bool CanProcess(Type type)
        {
            return typeof(ContentItem).IsAssignableFrom(type);
        }

        public override async Task<bool> AfterSave<T, TId>(T item, PropertyChange[] changes = null)
        {
            var content = item as ContentItem;
            if (content != null)
            {
                var sites = await _bioContext.Sites.Where(s => content.SiteIds.Contains(s.Id)).ToListAsync();
                foreach (var site in sites)
                {
                    var siteSettings = await _settingsProvider.Get<FacebookSiteSettings>(site);
                    if (!siteSettings.IsEnabled)
                    {
                        _logger.LogInformation($"Facebook is not enabled for site {site.Title}");
                        continue;
                    }

                    var facebookConfig = new FacebookServiceConfiguration
                    {
                        AccessToken = siteSettings.AccessToken,
                        ApiUrl = siteSettings.ApiUrl,
                        PageId = siteSettings.PageId
                    };

                    var itemSettings = await _settingsProvider.Get<FacebookContentSettings>(content, site.Id);

                    var hasChanges = changes != null && changes.Any(c => c.Name == nameof(content.Url));

                    if (!string.IsNullOrEmpty(itemSettings.PostId) && (hasChanges || !content.IsPublished))
                    {
                        var deleted = await _facebookService.DeletePost(itemSettings.PostId, facebookConfig);
                        if (!deleted)
                        {
                            throw new Exception("Can't delete content post from Facebook");
                        }

                        itemSettings.PostId = null;
                    }

                    if (content.IsPublished && (string.IsNullOrEmpty(itemSettings.PostId) || hasChanges))
                    {
                        var postId = await _facebookService.PostLink(new Uri($"{site.Url}{content.PublicUrl}"),
                            facebookConfig);
                        if (!string.IsNullOrEmpty(postId))
                        {
                            itemSettings.PostId = postId;
                        }
                    }

                    await _settingsProvider.Set(itemSettings, content, site.Id);
                }


                return true;
            }

            return false;
        }
    }
}