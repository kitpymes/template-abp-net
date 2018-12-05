using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Manager.EntityFramework;

namespace Manager.Migrator
{
    [DependsOn(typeof(ManagerDataModule))]
    public class ManagerMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<ManagerDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}