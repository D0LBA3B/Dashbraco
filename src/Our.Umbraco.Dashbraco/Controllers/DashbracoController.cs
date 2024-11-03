using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Our.Umbraco.Dashbraco.Interfaces;
using Our.Umbraco.Dashbraco.Models;
using Our.Umbraco.Dashbraco.Services;
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
        private readonly IMediaService _mediaService;
        private readonly IRelationService _relationService;
        private readonly MediaRemoveContextModel _mediaRemoveContext;

        public DashbracoController(AppCaches appCaches,
            IScopeProvider scopeProvider,
            IUserService userService,
            IBackOfficeSecurity security,
            IEntityService entityService,
            IHttpClientFactory httpClientFactory,
            IOptions<DashbracoSettings> settings,
            IMediaService mediaService,
            IRelationService relationService)
        {
            _appCaches = appCaches;
            _scopeProvider = scopeProvider;
            _userService = userService;
            _security = security;
            _entityService = entityService;
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
            _mediaService = mediaService;
            _relationService = relationService;
            _mediaRemoveContext = MediaRemoveContextModel.Current;
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

        [HttpGet]
        public IActionResult StartUnusedMediaReport()
        {
            if (!_mediaRemoveContext.IsProcessingMedia)
            {
                var unusedMediaService = new UnusedMediaService(_mediaService, _relationService);
                Thread backgroundGetMedia = new Thread(unusedMediaService.FindUnusedMedia)
                {
                    IsBackground = true,
                    Name = "UnusedMedia GetUnusedMedia"
                };
                backgroundGetMedia.Start();

                return Ok("Media report generation started");
            }
            return BadRequest("Media report is already being processed");
        }

        [HttpGet]
        public MediaReportModel GetUnusedMediaReportStatus()
        => new MediaReportModel
            {
                IsProcessingMedia = _mediaRemoveContext.IsProcessingMedia,
                Data = _mediaRemoveContext.UnusedMedia.Select(x => x.Model),
                TotalAmountOfMedia = _mediaRemoveContext.TotalAmountOfMedia,
                TotalUnusedMedia = _mediaRemoveContext.UnusedMedia.Count
            };

        public List<UnusedMediaModel> GetUnusedMediaReport()
        {
            if (!_mediaRemoveContext.IsProcessingMedia)
                return _mediaRemoveContext.UnusedMedia.Select(x => x.Model).ToList();
            return new List<UnusedMediaModel>();
        }

        [HttpPost]
        public IActionResult MoveItemToRecycling(int mediaId)
        {
            var mediaToDelete = _mediaService.GetById(mediaId);
            if (mediaToDelete == null)
                return NotFound($"Media with ID {mediaId} not found");

            _mediaService.MoveToRecycleBin(mediaToDelete);
            return Ok($"Media with ID {mediaId} moved to recycling bin");
        }
    }
}
