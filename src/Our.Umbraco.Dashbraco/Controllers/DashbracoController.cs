using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Our.Umbraco.Dashbraco.Models;
using System.Runtime;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.BackOffice.Filters;
using Umbraco.Cms.Web.Common.Attributes;

namespace Our.Umbraco.Dashbraco.Controllers
{
    [IsBackOffice]
    [JsonCamelCaseFormatter]
    public class DashbracoController : UmbracoAuthorizedJsonController
    {
        private readonly AppCaches _appCaches;
        private readonly IScopeProvider _scopeProvider;
        private readonly IUserService _userService;
        private readonly IBackOfficeSecurity _security;
        private readonly IEntityService _entityService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DashbracoSettings _settings;

        public DashbracoController(AppCaches appCaches,
            IScopeProvider scopeProvider,
            IUserService userService,
            IBackOfficeSecurity security,
            IEntityService entityService,
            IHttpClientFactory httpClientFactory,
            IOptions<DashbracoSettings> settings
        )
        {
            _appCaches = appCaches;
            _scopeProvider = scopeProvider;
            _userService = userService;
            _security = security;
            _entityService = entityService;
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
        }

        [HttpGet]
        public DashbracoSettings GetConfig() => _settings;

        [HttpGet]
        public GAnalyticsModel GetAnalyticsData()
        {
            var data = new GAnalyticsModel
            {
                Visitors = "125",
                PageViews = "512",
                AvgSessionDuration = 225 
            };
            return data;
        }
    }
}