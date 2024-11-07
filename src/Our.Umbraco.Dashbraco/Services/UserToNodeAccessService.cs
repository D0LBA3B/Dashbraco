using Our.Umbraco.Dashbraco.Interfaces;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Actions;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Services;

namespace Our.Umbraco.Dashbraco.Services
{
    internal class UserToNodeAccessHelper
    {
        private readonly IEntityService _entityService;
        private readonly AppCaches _appCaches;
        private EntityPermissionCollection _permissions;
        private int[] _userStartNodes;

        internal UserToNodeAccessHelper(
            IUser currentUser,
            IUserService userService,
            IEntityService entityService,
            AppCaches appCaches,
            IEnumerable<IUmbracoNodeWithPermissions> nodes)
        {
            _entityService = entityService;
            _appCaches = appCaches;
            _userStartNodes = currentUser.CalculateContentStartNodeIds(_entityService, appCaches);
            _permissions = userService.GetPermissions(currentUser, AllNodeIdsFromPath(nodes));
        }

        private int[] AllNodeIdsFromPath(IEnumerable<IUmbracoNodeWithPermissions> nodes)
        {
            List<int> ids = new List<int>();
            foreach (var node in nodes)
            {
                var nodeIds = node.NodePath.Split(',').Select(x => int.Parse(x));
                ids.AddRange(nodeIds);
            }
            return ids.Distinct().ToArray();
        }

        public bool HasAccessTo(IUmbracoNodeWithPermissions node)
        {
            var nodeIds = node.NodePath
                .Split(',')
                .Reverse()
                .Select(x => int.Parse(x))
                .Where(x => x > 0)
                .ToList();

            if (_userStartNodes.Length > 0)
            {
                bool onlyRootInList = _userStartNodes.Length == 1 && _userStartNodes[0] == Constants.System.Root;

                if (_userStartNodes.Length > 1 || !onlyRootInList)
                {
                    bool found = false;
                    foreach (var startNodeId in _userStartNodes)
                    {
                        var inPath = nodeIds.Contains(startNodeId);
                        if (inPath)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        return false;
                }

            }

            foreach (var id in nodeIds)
            {
                if (!HasPermissions(_permissions, id))
                    return false;
            }
            return true;
        }

        private bool HasPermissions(EntityPermissionCollection permissionResults, int nodeId)
        {
            var forNode = permissionResults.Where(x => x.EntityId == nodeId).ToList();
            var permissions = forNode.SelectMany(x => x.AssignedPermissions).ToList();
            var browseNodeAction = ActionBrowse.ActionLetter.ToString();
            return permissions.Contains(browseNodeAction);
        }
    }
}
