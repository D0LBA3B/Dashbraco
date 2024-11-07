using NPoco;
using Our.Umbraco.Dashbraco.Interfaces;
using Our.Umbraco.Dashbraco.Models.Dtos;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Our.Umbraco.Dashbraco.Services
{
    public class EntriesActivitiesService : IEntriesActivitiesService
    {
        private readonly IScopeProvider _scopeProvider;

        public EntriesActivitiesService(IScopeProvider scopeProvider) => _scopeProvider = scopeProvider;

        public List<LogEntryDto> GetEntries()
        {
            try
            {
                using (var scope = _scopeProvider.CreateScope())
                {
                    var coreSql = @"ul.[id]
                              ,ul.[userId]
                              ,ul.[NodeId]
                              ,ul.[entityType]
                              ,ul.[Datestamp]
                              ,ul.[logHeader]
                              ,ul.[logComment]
	                          ,un.[text] as NodeName
                              ,un.[path] as NodePath
                              ,ucs.[date] as NodeScheduledDate
	                          ,uu.userName
	                          ,uu.userEmail
	                          ,uu.avatar as userAvatar
                          FROM umbracoLog as ul
	                        INNER JOIN umbracoNode as un ON un.id = ul.NodeId
	                        INNER JOIN umbracoUser as uu ON uu.id = ul.userId
                            LEFT OUTER JOIN umbracoContentSchedule as ucs ON ucs.nodeId = ul.NodeId
                          WHERE 
	                        ul.userId is not NULL 
	                        AND ul.nodeId is not NULL
	                        AND ul.nodeId > 0 -- Only include actual nodes
	                        AND ul.entityType = 'Document' 

                          ORDER by ul.Datestamp DESC";


                    var sql = scope.Database.DatabaseType == DatabaseType.SQLite ? $"SELECT {coreSql} Limit 500" : $"Select Top(500) {coreSql}";
                    var res = scope.Database.Fetch<LogEntryDto>(sql);
                    return res;
                }
            }
            catch (Exception e)
            {
                return new List<LogEntryDto>();
            }
        }
    }
}
