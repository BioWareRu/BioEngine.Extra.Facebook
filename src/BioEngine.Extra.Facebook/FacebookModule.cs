using System;
using BioEngine.Core.Entities;
using BioEngine.Core.Modules;
using BioEngine.Core.Properties;
using BioEngine.Core.Repository;
using BioEngine.Extra.Facebook.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BioEngine.Extra.Facebook
{
    public class FacebookModule : BioEngineModule<FacebookModuleConfig>
    {
        protected override void CheckConfig()
        {
            if (Config.Url == null)
            {
                throw new ArgumentException("Facebook api url is not set");
            }

            if (string.IsNullOrEmpty(Config.PageId))
            {
                throw new ArgumentException("Facebook pageId is not set");
            }

            if (string.IsNullOrEmpty(Config.AccessToken))
            {
                throw new ArgumentException("Facebook access token is not set");
            }
        }

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration,
            IHostEnvironment environment)
        {
            services.AddSingleton(Config);
            services.AddSingleton<FacebookService>();
            services.AddScoped<IRepositoryHook, FacebookContentHook>();

            PropertiesProvider.RegisterBioEngineContentProperties<FacebookContentPropertiesSet>("facebookcontent");
            PropertiesProvider.RegisterBioEngineProperties<FacebookSitePropertiesSet, Site>("facebooksite");
        }
    }

    public class FacebookModuleConfig
    {
        public FacebookModuleConfig(Uri url, string pageId, string accessToken)
        {
            Url = url;
            PageId = pageId;
            AccessToken = accessToken;
        }

        public Uri Url { get; }
        public string PageId { get; }
        public string AccessToken { get; }
    }
}
