using Abp.Authorization;
using Manager.Core.Configuration.Roles;
using Manager.Core.Configuration.MultiTenancy;
using Manager.Core.Configuration.Users;

namespace Manager.Core.Common.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
