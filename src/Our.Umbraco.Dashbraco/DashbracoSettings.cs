namespace Our.Umbraco.Dashbraco
{
    public class DashbracoSettings
    {
        public string[] DefaultWidgets { get; set; } = ["Analytics", "PictureOfTheDay", "UnusedMedia"];
        public int RefreshInterval { get; set; } = 300;
        public string GoogleAnalyticsPropertyId { get; set; } = String.Empty;
        public string CredentialsPath { get; set; } = String.Empty;
    }
}