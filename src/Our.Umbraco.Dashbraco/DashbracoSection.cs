using Umbraco.Cms.Core.Sections;

namespace Our.Umbraco.Dashbraco
{
    public class DashbracoSection : ISection
    {
        public string Alias => DashConstants.DashbracoDefs.DashbracoSectionAlias;
        public string Name => DashConstants.DashbracoDefs.DashbracoSection;
    }
}