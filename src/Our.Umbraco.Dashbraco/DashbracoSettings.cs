namespace Our.Umbraco.Dashbraco
{
    public class DashbracoSettings
    {
        public string[] DefaultWidgets { get; set; } = ["Analytics", "PictureOfTheDay", "UnusedMedia"];
        public int RefreshInterval { get; set; } = 300;
        public string GoogleAnalyticsPropertyId { get; set; } = String.Empty;
        public DashColors Styles { get; set; } = new DashColors();
        public Dictionary<string, string> GoogleCredentials { get; set; }
    }

    public class DashColors
    {
        public string PrimaryColor { get; set; } = "#f3f3f3";
        public string SecondaryColor { get; set; } = "#f3f3f3";
        public string TextColor { get; set; } = "#333333";
        public string ActiveTabColor { get; set; } = "#007ACC";
        public string InactiveTabColor { get; set; } = "#CCCCCC";
        public string BackgroundColor { get; set; } = "#FFFFFF";
        public string BorderColor { get; set; } = "#E0E0E0";
        public string HoverColor { get; set; } = "#005A9E";
        public string ButtonColor { get; set; } = "#007ACC";
        public string ButtonTextColor { get; set; } = "#FFFFFF";
        public string SuccessColor { get; set; } = "#28A745";
        public string WarningColor { get; set; } = "#FFC107";
        public string ErrorColor { get; set; } = "#DC3545";
        public string LinkColor { get; set; } = "#007BFF";
        public string LinkHoverColor { get; set; } = "#0056b3";
    }
}