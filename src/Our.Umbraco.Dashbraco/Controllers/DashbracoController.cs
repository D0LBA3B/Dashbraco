using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Our.Umbraco.Dashbraco.Interfaces;
using Our.Umbraco.Dashbraco.Models;
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

        public DashbracoController(IOptions<DashbracoSettings> settings,
            IMediaService mediaService,
            IUnusedMediaService unusedMediaService,
            IGoogleAnalyticsService googleAnalyticsService)
        {
            _settings = settings.Value;
            _mediaService = mediaService;
            _mediaRemoveContext = MediaRemoveContextModel.Current;
            _unusedMediaService = unusedMediaService;
            _googleAnalyticsService = googleAnalyticsService;
        }

        [HttpGet]
        public DashbracoSettings GetConfig() => _settings;

        [HttpGet]
        public async Task<GAnalyticsModel> GetAnalyticsData()
        => await _googleAnalyticsService.GetMonthlyAnalyticsDataAsync();

        [HttpGet]
        public async Task<List<DailyActiveUsersModel>> GetDailyActiveUsers()
        => await _googleAnalyticsService.GetDailyActiveUsersAsync();

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
    }
}
