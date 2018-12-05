using Manager.EntityFramework;
using EntityFramework.DynamicFilters;
using Manager.Core.Configuration.Roles;

namespace Manager.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly ManagerDbContext _context;
        private readonly string _emailMaster;

        public InitialHostDbBuilder(ManagerDbContext context, string emailMaster)
        {
            _context = context;
            _emailMaster = emailMaster;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            // Host user
            new HostRoleAndUserCreator(_context, _emailMaster, StaticRoleNames.Host.Master).Create();

            // Default tenant
            new DefaultTenantCreator(_context).Create();
        }
    }
}
