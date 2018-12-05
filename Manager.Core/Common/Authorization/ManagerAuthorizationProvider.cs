using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Manager.Core.Common.Authorization
{
    public class ManagerAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //Common permissions
            var pages = context.GetPermissionOrNull(PermissionNames.Pages);
            if (pages == null)
            {
                pages = context.CreatePermission(PermissionNames.Pages, L("Pages"));
            }

            //Host permissions
            var tenants = pages.CreateChildPermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

			 #region Common Pages

            // Page Configuration
            var pagesConfiguration = pages.CreateChildPermission(PermissionNames.Pages_Configuration);
            
            var pagesUsers = pagesConfiguration.CreateChildPermission(PermissionNames.Pages_Configuration_Users);
            pagesUsers.CreateChildPermission(PermissionNames.Pages_Configuration_Users_Create)
                .CreateChildPermission(PermissionNames.Pages_Configuration_Users_Edit)
                .CreateChildPermission(PermissionNames.Pages_Configuration_Users_Delete);

            var pagesRoles = pagesConfiguration.CreateChildPermission(PermissionNames.Pages_Configuration_Roles);
            pagesRoles.CreateChildPermission(PermissionNames.Pages_Configuration_Roles_Create)
                .CreateChildPermission(PermissionNames.Pages_Configuration_Roles_Edit)
                .CreateChildPermission(PermissionNames.Pages_Configuration_Roles_Delete);

            #endregion
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ManagerConsts.LocalizationSourceName);
        }
    }
}
