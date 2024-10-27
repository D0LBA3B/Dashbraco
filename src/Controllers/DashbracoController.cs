using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Umbraco.Cms.Web.Common.Controllers;

namespace Dashbraco.Controllers
{
    public class DashbracoApiController : UmbracoAuthorizedApiController
    {
        private readonly IConfiguration _configuration;

        public DashbracoApiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetDashbracoSettings()
        {
            try
            {
                var settings = _configuration.GetSection("DashbracoSettings").Get<DashbracoSettings>()
                    ?? new DashbracoSettings
                    {
                        EnabledModules = new[] { "Page Views", "User Engagement" },
                        RefreshInterval = 300
                    };

                return new JsonResult(settings);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }

    public class DashbracoSettings
    {
        public string[] EnabledModules { get; set; }
        public int RefreshInterval { get; set; }
    }
}
