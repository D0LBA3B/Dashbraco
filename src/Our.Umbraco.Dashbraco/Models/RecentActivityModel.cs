namespace Our.Umbraco.Dashbraco.Models
{
    public class RecentActivityModel
    {
        public string ActivityType { get; set; }

        public string NodeName { get; set; }
        public int NodeId { get; set; }

        public UserModel User { get; set; }
        public DateTime Datestamp { get; set; }
        public DateTime? ScheduledPublishDate { get; set; }
    }
}