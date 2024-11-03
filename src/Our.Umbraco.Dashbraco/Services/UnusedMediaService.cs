using Our.Umbraco.Dashbraco.Interfaces;
using Our.Umbraco.Dashbraco.Models;
using System.Collections.Concurrent;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace Our.Umbraco.Dashbraco.Services
{
    public class UnusedMediaService : IUnusedMediaService
    {
        private readonly IMediaService _mediaService;
        private readonly IRelationService _relationService;
        private readonly MediaRemoveContextModel _mediaRemoveContext;

        public UnusedMediaService(IMediaService mediaService, IRelationService relationService)
        {
            _mediaService = mediaService;
            _relationService = relationService;
            _mediaRemoveContext = MediaRemoveContextModel.Current;
        }

        private void GetUnusedMediaItems(MediaItemWrapperModel root)
        {
            var children = _mediaService.GetPagedChildren(root.Media.Id, 0, 10000, out _);
            _mediaRemoveContext.TotalAmountOfMedia += children.Count();

            foreach (var mediaItem in children)
            {
                GetUnusedMediaItems(new MediaItemWrapperModel(mediaItem, root));
            }

            if (children.Any()) return;

            var mediaRelations = _relationService.GetByChildId(root.Media.Id);
            if (!mediaRelations.Any() && root.Media.ContentType.Alias != "Folder")
            {
                _mediaRemoveContext.UnusedMedia.Add(root);
            }
        }

        public void FindUnusedMedia()
        {
            _mediaRemoveContext.UnusedMedia = new ConcurrentBag<MediaItemWrapperModel>();
            _mediaRemoveContext.IsProcessingMedia = true;
            _mediaRemoveContext.TotalAmountOfMedia = 0;

            var mediaItems = _mediaService.GetRootMedia();
            _mediaRemoveContext.TotalAmountOfMedia += mediaItems.Count();

            foreach (var mediaItem in mediaItems)
            {
                GetUnusedMediaItems(new MediaItemWrapperModel(mediaItem));
            }

            _mediaRemoveContext.IsProcessingMedia = false;
        }
    }
}
