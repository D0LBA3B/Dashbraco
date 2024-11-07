using Our.Umbraco.Dashbraco.Models.Dtos;
using Our.Umbraco.Dashbraco.Models;
using Our.Umbraco.Dashbraco;
using Umbraco.Cms.Core.Cache;
using UserExtensions = Our.Umbraco.Dashbraco.Extensions.UserExtensions;
using UserModel = Our.Umbraco.Dashbraco.Models.UserModel;

namespace Our.Umbraco.TheDashboard.Mapping
{
	public class LogEntryToRecentActivityMapper
	{
		private readonly AppCaches _appCaches;
        private readonly IHttpClientFactory _httpClientFactory;

        public LogEntryToRecentActivityMapper(AppCaches appCaches, IHttpClientFactory httpClientFactory)
        {
            _appCaches = appCaches;
            _httpClientFactory = httpClientFactory;
        }

		public RecentActivityModel Map(LogEntryDto dto)
		{
			var dashboardLogEntryType = GetLogEntryType(dto);

			if (string.IsNullOrEmpty(dashboardLogEntryType))
			{
				return null;
			}

			return new RecentActivityModel()
			{
				NodeId = dto.NodeId,
				NodeName = dto.NodeName,
				Datestamp = dto.Datestamp,
				ScheduledPublishDate = dto.NodeScheduledDate,
				ActivityType = dashboardLogEntryType,
				User = new UserModel()
				{
					Name = dto.UserName,
					Avatar = UserExtensions.GetUserAvatarUrls(dto.UserId, dto.UserEmail, dto.UserAvatar, _appCaches.RuntimeCache,_httpClientFactory)
				}
			};
		}

		private string GetLogEntryType(LogEntryDto dto)
		{
			if (dto.LogHeader == DashConstants.UmbracoLogHeaders.Publish ||
				dto.LogHeader == DashConstants.UmbracoLogHeaders.PublishVariant)
			{
				return DashConstants.ActivityTypes.Publish;
			}

			if (dto.LogHeader == DashConstants.UmbracoLogHeaders.Save ||
				dto.LogHeader == DashConstants.UmbracoLogHeaders.SaveVariant)
			{
				if (dto.NodeScheduledDate.HasValue)
				{
					return DashConstants.ActivityTypes.SaveAndScheduled;
				}

				return DashConstants.ActivityTypes.Save;
			}

			if (dto.LogHeader == DashConstants.UmbracoLogHeaders.Unpublish ||
				dto.LogHeader == DashConstants.UmbracoLogHeaders.UnpublishVariant)
			{
				return DashConstants.ActivityTypes.Unpublish;
			}

			if (dto.LogHeader == DashConstants.UmbracoLogHeaders.RollBack)
			{
				return DashConstants.ActivityTypes.RollBack;
			}

			if (dto.LogHeader == DashConstants.UmbracoLogHeaders.Delete && dto.LogComment.Contains("Trashed"))
			{
				return DashConstants.ActivityTypes.RecycleBin;
			}
			return string.Empty;
		}
	}
}
