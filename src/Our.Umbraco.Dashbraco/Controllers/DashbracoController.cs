using Microsoft.AspNetCore.Mvc;
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

        public DashbracoController(AppCaches appCaches,
            IScopeProvider scopeProvider,
            IUserService userService,
            IBackOfficeSecurity security,
            IEntityService entityService,
            IHttpClientFactory httpClientFactory
        )
        {
            _appCaches = appCaches;
            _scopeProvider = scopeProvider;
            _userService = userService;
            _security = security;
            _entityService = entityService;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public string GetAllRecentActivities()
        {

            return "model";
        }

        public string GetMyRecentActivities()
        {
            return "";
        }
    }
}