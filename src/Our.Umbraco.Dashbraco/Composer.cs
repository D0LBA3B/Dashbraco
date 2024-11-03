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
            builder.Dashboards().Add<DashbracoDashboard>();
        }
    }
}