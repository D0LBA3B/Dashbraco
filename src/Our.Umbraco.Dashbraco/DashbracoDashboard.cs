using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Dashboards;

namespace Our.Umbraco.Dashbraco
{
    [Weight(-10000)]
    public class DashbracoDashboard : IDashboard
    {
        public string Alias => DashConstants.DashbracoDefs.DashbracoDashboard;
        public string View => "/App_Plugins/Our.Umbraco.Dashbraco/Dashbraco.html";

        public string[] Sections => new[]
        {
            global::Umbraco.Cms.Core.Constants.Applications.Content
        };

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();
    }
}
