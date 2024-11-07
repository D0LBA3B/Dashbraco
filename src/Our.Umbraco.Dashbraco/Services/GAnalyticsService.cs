using Google.Apis.AnalyticsData.v1beta;
using Google.Apis.AnalyticsData.v1beta.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Our.Umbraco.Dashbraco.Interfaces;
using Our.Umbraco.Dashbraco.Models;

namespace Our.Umbraco.Dashbraco.Services
{
    public class GoogleAnalyticsService : IGoogleAnalyticsService
    {
        private readonly DashbracoSettings _settings;
        private readonly AnalyticsDataService _analyticsDataService;
        private string _error;

        public GoogleAnalyticsService(IOptions<DashbracoSettings> settings)
        {
            _settings = settings.Value;

            try
            {
                var jsonCredentials = JsonConvert.SerializeObject(settings.Value.GoogleCredentials);
                var credential = GoogleCredential.FromJson(jsonCredentials)
                    .CreateScoped(AnalyticsDataService.Scope.AnalyticsReadonly);
                _analyticsDataService = new AnalyticsDataService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Dashbraco"
                });
            }
            catch (Exception ex)
            {
                _error = $": \"{ex.Message}\"";
            }
        }

        public Task<(bool, string)> CheckConfig()
        {
            if (_analyticsDataService is null)
                return Task.FromResult((false, "Google Analytics credentials are not set or are invalid" + _error));
            return Task.FromResult((true, "Google Analytics credentials are set and valid."));
        }

        public async Task<GAnalyticsModel> GetMonthlyAnalyticsDataAsync()
        {
            if (_analyticsDataService is null)
                return new GAnalyticsModel();

            var dateRange = new DateRange
            {
                StartDate = "30daysAgo",
                EndDate = "today"
            };

            var metrics = new[]
            {
                new Metric { Name = "activeUsers" },
                new Metric { Name = "screenPageViews" },
                new Metric { Name = "sessions" },
                new Metric { Name = "bounceRate" }
            };

            var request = new RunReportRequest
            {
                DateRanges = new[] { dateRange },
                Metrics = metrics
            };

            var response = await _analyticsDataService.Properties.RunReport(request, "properties/" + _settings.GoogleAnalyticsPropertyId).ExecuteAsync();

            if(response is null || response?.Rows?.Count == 0)
                return new GAnalyticsModel();

            var users = response.Rows[0].MetricValues[0].Value;
            var pageViews = response.Rows[0].MetricValues[1].Value;
            var sessions = response.Rows[0].MetricValues[2].Value;
            var bounceRate = response.Rows[0].MetricValues[3].Value;

            return new GAnalyticsModel
            {
                Visitors = users,
                PageViews = pageViews,
                Sessions = sessions,
                BounceRate = bounceRate
            };
        }

        public async Task<List<DailyActiveUsersModel>> GetDailyActiveUsersAsync()
        {
            if (_analyticsDataService is null)
                return new List<DailyActiveUsersModel>();

            var dateRange = new DateRange
            {
                StartDate = "30daysAgo",
                EndDate = "today"
            };

            var metrics = new[] { new Metric { Name = "activeUsers" } };
            var dimensions = new[] { new Dimension { Name = "date" } };

            var request = new RunReportRequest
            {
                DateRanges = new[] { dateRange },
                Metrics = metrics,
                Dimensions = dimensions
            };

            var response = await _analyticsDataService.Properties.RunReport(request, "properties/" + _settings.GoogleAnalyticsPropertyId).ExecuteAsync();

            if (response is null || response?.Rows?.Count == 0)
                return new List<DailyActiveUsersModel>();

            var dailyData = response.Rows.Select(row => new DailyActiveUsersModel
            {
                Date = DateTime.ParseExact(row.DimensionValues[0].Value, "yyyyMMdd", null),
                ActiveUsers = int.Parse(row.MetricValues[0].Value)
            }).ToList();

            return dailyData;
        }
    }
}
