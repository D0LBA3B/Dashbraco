using NPoco;
using Our.Umbraco.Dashbraco.Interfaces;
using Our.Umbraco.Dashbraco.Models;
using Our.Umbraco.Dashbraco.Models.Dtos;
using Umbraco.Cms.Core;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Our.Umbraco.Dashbraco.Services;

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


                var sql = scope.Database.DatabaseType == DatabaseType.SQLite
                    ? $"SELECT {coreSql} Limit 500"
                    : $"Select Top(500) {coreSql}";
                var res = scope.Database.Fetch<LogEntryDto>(sql);
                return res;
            }
        }
        catch (Exception e)
        {
            return new List<LogEntryDto>();
        }
    }

    public StatsOverviewModel GetItemsInRecycleBin()
    {
        try
        {
            using var scope = _scopeProvider.CreateScope();
            var sql = @"SELECT count(un.[id]) FROM umbracoNode AS un
                               WHERE un.nodeObjectType = @0 	
	                           AND un.trashed = 1";
            var res = scope.Database.ExecuteScalar<int>(sql, Constants.ObjectTypes.Document);
            return new StatsOverviewModel
            {
                Count = res,
                Url = "/umbraco#/content/content/recyclebin",
                Text = "Items in recycle bin"
            };
        }
        catch (Exception e)
        {
            return new StatsOverviewModel { Count = 0 };
        }
    }

    public StatsOverviewModel GetTotalMembers()
    {
        try
        {
            using var scope = _scopeProvider.CreateScope();
            var count = scope.Database.ExecuteScalar<int>(@"select COUNT(nodeId) from cmsMember");
            return new StatsOverviewModel
            {
                Count = count,
                Url = "/umbraco#/member/member/list/all-members",
                Text = "Total members"
            };
        }
        catch (Exception)
        {
            return new StatsOverviewModel { Count = 0 };
        }
    }

    public StatsOverviewModel GetTotalMediaItems()
    {
        try
        {
            using var scope = _scopeProvider.CreateScope();
            var count = scope.Database.ExecuteScalar<int>(@"SELECT COUNT(*) FROM (
                         SELECT ""umbracoNode"".""id"" AS ""Id""
                         FROM ""umbracoNode""
                                  INNER JOIN ""umbracoContent""
                                             ON (""umbracoNode"".""id"" = ""umbracoContent"".""nodeId"")
                                  INNER JOIN ""umbracoContentVersion""
                                             ON ((""umbracoNode"".""id"" = ""umbracoContentVersion"".""nodeId"") AND ""umbracoContentVersion"".""current"" = 1)
                         WHERE (((""umbracoNode"".""nodeObjectType"" = 'B796F64C-1F99-4FFB-B886-4BF4BC011A9C') AND (""umbracoNode"".""trashed"" = 0)))
                     ) npoco_tbl");
            return new StatsOverviewModel
            {
                Count = count,
                Url = "/umbraco#/media",
                Text = "Total media items"
            };
        }
        catch (Exception)
        {
            return new StatsOverviewModel { Count = 0 };
        }
    }

    public StatsOverviewModel GetTotalElements()
    {
        try
        {
            using var scope = _scopeProvider.CreateScope();
            var sql = @"SELECT count(un.[id])
                      FROM umbracoNode AS un
	                    INNER JOIN umbracoDocument as ud on ud.nodeId = un.id
                      WHERE 
	                    un.nodeObjectType = @0
                        AND un.trashed = 0
	                    AND ud.published = 1";
            var res = scope.Database.ExecuteScalar<int>(sql, Constants.ObjectTypes.Document);
            return new StatsOverviewModel
            {
                Count = res,
                Url = String.Empty,
                Text = "Total elements"
            };
        }
        catch (Exception)
        {
            return new StatsOverviewModel { Count = 0 };
        }
    }
}