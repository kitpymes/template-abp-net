using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using Manager.Migrations.SeedData;
using EntityFramework.DynamicFilters;
using Manager.Core.Configuration.Roles;

namespace Manager.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<EntityFramework.ManagerDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Manager";
        }

        protected override void Seed(EntityFramework.ManagerDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                // Initial Host
                new InitialHostDbBuilder(context, "sferrari.net@gmail.com").Create();

                // Tenant users
                new TenantRoleAndUserBuilder(context, 1, "admin@gmail.com", StaticRoleNames.Tenants.Admin).Create();
                new TenantRoleAndUserBuilder(context, 1, "user@gmail.com", StaticRoleNames.Tenants.User).Create();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
