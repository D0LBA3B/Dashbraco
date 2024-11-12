using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Services;
using static Our.Umbraco.Dashbraco.DashConstants;

public class UserGroupComponent : IComponent
{
    private readonly IUserService _userService;

    public UserGroupComponent(IUserService userService)
    {
        _userService = userService;
    }

    public void Initialize()
    {
        var userGroups = _userService.GetAllUserGroups();
        foreach (var group in userGroups)
        {
            if (!group.AllowedSections.Contains(DashbracoDefs.DashbracoSectionAlias))
            {
                group.AddAllowedSection(DashbracoDefs.DashbracoSectionAlias);
                _userService.Save(group);
            }
        }
    }

    public void Terminate()
    { }
}