using System.Collections.Concurrent;

namespace Our.Umbraco.Dashbraco.Models
{
    public class MediaRemoveContextModel
    {
        private static MediaRemoveContextModel instance;
        public MediaRemoveContextModel()
        {
            UnusedMedia = new ConcurrentBag<MediaItemWrapperModel>();
            IsProcessingMedia = false;
            instance = this;
        }

        public static MediaRemoveContextModel Current => instance ?? new MediaRemoveContextModel();
        public ConcurrentBag<MediaItemWrapperModel> UnusedMedia { get; set; }
        public bool IsProcessingMedia { get; set; }
        public int TotalAmountOfMedia { get; set; }
    }
}
