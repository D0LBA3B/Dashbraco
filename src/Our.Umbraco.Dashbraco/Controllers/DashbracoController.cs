using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Our.Umbraco.Dashbraco.Interfaces;
using Our.Umbraco.Dashbraco.Models;
using Our.Umbraco.Dashbraco.Models.Dtos;
using Our.Umbraco.Dashbraco.Services;
using Our.Umbraco.TheDashboard.Mapping;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.BackOffice.Filters;
using Umbraco.Cms.Web.Common.Attributes;

namespace Our.Umbraco.Dashbraco.Controllers
{
    [IsBackOffice]
    [JsonCamelCaseFormatter]
    public class DashbracoController : UmbracoAuthorizedJsonController
    {
        private readonly DashbracoSettings _settings;
        private readonly IMediaService _mediaService;
        private readonly IUnusedMediaService _unusedMediaService;
        private readonly MediaRemoveContextModel _mediaRemoveContext;
        private readonly IGoogleAnalyticsService _googleAnalyticsService;
        private readonly IEntriesActivitiesService _entriesActivitiesService;
        private readonly IUserService _userService;
        private readonly IEntityService _entityService;
        private readonly AppCaches _appCaches;
        private readonly IBackOfficeSecurity _backOfficeSecurity;
        private readonly IHttpClientFactory _httpClientFactory;

        public DashbracoController(IOptions<DashbracoSettings> settings,
            IMediaService mediaService,
            IUnusedMediaService unusedMediaService,
            IGoogleAnalyticsService googleAnalyticsService,
            IEntriesActivitiesService entreEntriesActivitiesService,
            IUserService userService,
            IEntityService entityService,
            AppCaches appCaches,
            IBackOfficeSecurity backOfficeSecurity,
            IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            _mediaService = mediaService;
            _mediaRemoveContext = MediaRemoveContextModel.Current;
            _unusedMediaService = unusedMediaService;
            _googleAnalyticsService = googleAnalyticsService;
            _entriesActivitiesService = entreEntriesActivitiesService;
            _userService = userService;
            _entityService = entityService;
            _appCaches = appCaches;
            _backOfficeSecurity = backOfficeSecurity;
            _httpClientFactory = httpClientFactory;
        }

        #region Configuration

        [HttpGet]
        public DashbracoSettings GetConfig() => _settings;

        [HttpGet]
        public async Task<(bool, string)> CheckGoogleAnalyticsConfig()
        => await _googleAnalyticsService.CheckConfig();

        #endregion

        #region Analytics
        [HttpGet]
        public async Task<GAnalyticsModel> GetAnalyticsData()
        => await _googleAnalyticsService.GetMonthlyAnalyticsDataAsync();

        [HttpGet]
        public async Task<List<DailyActiveUsersModel>> GetDailyActiveUsers()
        => await _googleAnalyticsService.GetDailyActiveUsersAsync();
        #endregion

        #region UnusedMedia

        [HttpGet]
        public IActionResult StartUnusedMediaReport()
        {
            if (!_mediaRemoveContext.IsProcessingMedia)
            {
                Thread backgroundGetMedia = new Thread(_unusedMediaService.FindUnusedMedia)
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
        #endregion

        #region EntriesActivities

        [HttpGet]
        public RecentActivitiesModel GetAllRecentActivities()
        {
            var model = new RecentActivitiesModel()
            {
                AllItems = new List<RecentActivityModel>(),
                YourItems = new List<RecentActivityModel>()
            };

            var allRecentDtos = _entriesActivitiesService.GetEntries();
            var accessService = new UserToNodeAccessHelper(_backOfficeSecurity.CurrentUser, _userService, _entityService, _appCaches, allRecentDtos);
            var filteredDtos = allRecentDtos.Where(x => accessService.HasAccessTo(x)).ToList();

            model.AllItems = CreateFrontendModelsFrom(filteredDtos);
            var filteredMyActivitiesDtos = filteredDtos.Where(x => x.UserId == _backOfficeSecurity?.CurrentUser?.Id).ToList();
            model.YourItems = CreateFrontendModelsFrom(filteredMyActivitiesDtos);

            return model;
        }

        #endregion

        #region Methods

        private List<RecentActivityModel> CreateFrontendModelsFrom(List<LogEntryDto> dtos)
        {
            var maxCount = 10;
            var mapper = new LogEntryToRecentActivityMapper(_appCaches, _httpClientFactory);

            var list = new List<RecentActivityModel>();
            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];
                if (!list.Any(x => x.NodeId == dto.NodeId))
                {
                    var vm = mapper.Map(dto);
                    if (vm != null)
                        list.Add(vm);
                }
                if (list.Count >= maxCount)
                    return list;
            }
            return list;
        }

        #endregion
    }
}
