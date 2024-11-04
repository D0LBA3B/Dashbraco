namespace Our.Umbraco.Dashbraco.Models
{
    public class GAnalyticsModel
    {
        public string Visitors { get; set; }
        public string PageViews { get; set; }
        public string Sessions { get; set; }
        public string BounceRate { get; set; }
    }

    public class DailyActiveUsersModel
    {
        public DateTime Date { get; set; }
        public int ActiveUsers { get; set; }
    }
}