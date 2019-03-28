using System;
using BioEngine.Core.Entities;
using BioEngine.Core.Modules;
using BioEngine.Core.Properties;
using BioEngine.Core.Repository;
using BioEngine.Extra.Facebook.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.Extra.Facebook
{
    public class FacebookModule : BioEngineModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostingEnvironment environment)
        {
            services.Configure<FacebookServiceConfiguration>(config =>
            {
                var parsed = Uri.TryCreate(configuration["BE_FACEBOOK_API_URL"], UriKind.Absolute,
                    out var url);
                if (!parsed)
                {
                    throw new ArgumentException(
                        $"Facebook api url is incorrect: {configuration["BE_FACEBOOK_API_URL"]}");
                }

                config.ApiUrl = url;
                config.PageId = configuration["BE_FACEBOOK_PAGE_ID"];
                config.AccessToken = configuration["BE_FACEBOOK_ACCESS_TOKEN"];
            });
            services.AddSingleton<FacebookService>();
            services.AddScoped<IRepositoryFilter, FacebookContentFilter>();

            PropertiesProvider.RegisterBioEngineContentProperties<FacebookContentPropertiesSet>();
            PropertiesProvider.RegisterBioEngineProperties<FacebookSitePropertiesSet, Site>();
        }
    }
}
