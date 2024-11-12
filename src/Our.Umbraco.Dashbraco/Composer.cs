using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Dashbraco.Interfaces;
using Our.Umbraco.Dashbraco.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.Dashbraco
{
    public class Composer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.Configure<DashbracoSettings>(builder.Config.GetSection("Dashbraco"));

            builder.Services.AddScoped<IUnusedMediaService, UnusedMediaService>();
            builder.Services.AddSingleton<IGoogleAnalyticsService, GoogleAnalyticsService>();
            builder.Services.AddSingleton<IEntriesActivitiesService, EntriesActivitiesService>();

            var dashbracoSettings = builder.Config.GetSection("Dashbraco").Get<DashbracoSettings>();
            Console.WriteLine(dashbracoSettings.DisplayAsDashboard);
            if (dashbracoSettings != null && !dashbracoSettings.DisplayAsDashboard)
            {
                builder.Sections().Append<DashbracoSection>();
                builder.Components().Append<UserGroupComponent>();
                builder.Dashboards().Remove<DashbracoDashboard>();
            }
            else
            {
                builder.Dashboards().Add<DashbracoDashboard>();
                builder.Sections().Remove<DashbracoSection>();
            }
        }

    }
}