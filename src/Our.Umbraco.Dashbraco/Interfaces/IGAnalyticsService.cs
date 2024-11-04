using Our.Umbraco.Dashbraco.Models;

namespace Our.Umbraco.Dashbraco.Interfaces
{
    public interface IGoogleAnalyticsService
    {
        public Task<GAnalyticsModel> GetMonthlyAnalyticsDataAsync();
        public Task<List<DailyActiveUsersModel>> GetDailyActiveUsersAsync();
    }
}
