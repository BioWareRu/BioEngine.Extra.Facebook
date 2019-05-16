using BioEngine.Core.DB;
using BioEngine.Core.Entities;
using BioEngine.Core.Modules;
using BioEngine.Core.Properties;
using BioEngine.Core.Publishers;
using BioEngine.Extra.Facebook.Entities;
using BioEngine.Extra.Facebook.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BioEngine.Extra.Facebook
{
    public class FacebookModule : BioEngineModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration,
            IHostEnvironment environment)
        {
            services.AddSingleton<FacebookService>();
            services.AddScoped<IContentPublisher<FacebookConfig>, FacebookContentPublisher>();
            services.AddScoped<FacebookContentPublisher>();

            PropertiesProvider.RegisterBioEngineProperties<FacebookSitePropertiesSet, Site>("facebooksite");
        }

        public override void RegisterEntities(BioEntitiesManager entitiesManager)
        {
            base.RegisterEntities(entitiesManager);
            entitiesManager.Register<FacebookPublishRecord>();
        }
    }
}
