using Newtonsoft.Json;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace Our.Umbraco.Dashbraco.Models
{
    public class MediaItemWrapperModel
    {
        public IMedia Media { get; set; }
        public UnusedMediaModel Model { get; set; }

        public MediaItemWrapperModel(IMedia media, MediaItemWrapperModel previous)
        {
            Media = media;
            Model = new UnusedMediaModel
            {
                Name = media.Name,
                Path = $"{previous.Model.Path}/{media.Name}",
                Id = media.Id,
                Source = media.ContentType.Alias != "Image" 
                    ? media.GetValue<string>("umbracoFile") 
                    : JsonConvert.DeserializeObject<ImageCropperValue>(media.GetValue<string>("umbracoFile")).Src,
                ParentId = media.ParentId
            };
        }

        public MediaItemWrapperModel(IMedia media)
        {
            Media = media;
            Model = new UnusedMediaModel
            {
                Name = media.Name,
                Path = $"{media.Name}",
                Id = media.Id,
                Source = media.HasProperty("umbracoFile") ? media.GetValue<string>("umbracoFile") : null,
                ParentId = media.ParentId
            };
        }
    }
}
