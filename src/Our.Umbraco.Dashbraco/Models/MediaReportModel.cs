namespace Our.Umbraco.Dashbraco.Models
{
    public class MediaReportModel
    {
        public bool IsProcessingMedia { get; set; }
        public IEnumerable<UnusedMediaModel> Data { get; set; }
        public int TotalAmountOfMedia { get; set; }
        public int TotalUnusedMedia { get; set; }
    }
}