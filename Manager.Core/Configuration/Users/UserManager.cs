using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using Manager.Core.Configuration.Roles;
using Manager.Core.Configuration.MultiTenancy;
using Abp.Localization;
using Abp.IdentityFramework;

namespace Manager.Core.Configuration.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {
        public UserManager(
            UserStore store,
            RoleManager roleManager,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ILocalizationManager localizationManager,
            IdentityEmailMessageService emailService,
            ISettingManager settingManager,
            IUserTokenProviderAccessor userTokenProviderAccessor
            )
            : base(
            store,
            roleManager,
            permissionManager,
            unitOfWorkManager,
            cacheManager,
            organizationUnitRepository,
            userOrganizationUnitRepository,
            organizationUnitSettings,
            localizationManager,
            emailService,
            settingManager,
            userTokenProviderAccessor)
        {
        }
    }
}