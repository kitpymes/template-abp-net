using Abp.MultiTenancy;
using Abp.Zero.Configuration;

namespace Manager.Core.Configuration.Roles
{
    public static class AppRoleConfig
    {
        public static void Configure(IRoleManagementConfig roleManagementConfig)
        {
            //Static host roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Host.Master,
                    MultiTenancySides.Host)
                );

            //Static tenant roles
            roleManagementConfig.StaticRoles.AddRange(new System.Collections.Generic.List<StaticRoleDefinition>{
                 new StaticRoleDefinition(
                    StaticRoleNames.Tenants.Admin,
                    MultiTenancySides.Tenant
                 ),
                 new StaticRoleDefinition(
                    StaticRoleNames.Tenants.User,
                    MultiTenancySides.Tenant
                 )
            });
        }
    }
}
