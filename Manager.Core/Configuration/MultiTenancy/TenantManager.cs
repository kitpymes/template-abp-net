using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Manager.Core.Configuration.Roles;
using Manager.Core.Configuration.Editions;
using Manager.Core.Configuration.Users;

namespace Manager.Core.Configuration.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore
            ) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager,
                featureValueStore
            )
        {
        }
    }
}