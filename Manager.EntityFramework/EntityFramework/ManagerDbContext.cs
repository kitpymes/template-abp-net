using System.Data.Common;
using Abp.Zero.EntityFramework;
using Manager.Core.Configuration.Roles;
using Manager.Core.Configuration.MultiTenancy;
using Manager.Core.Configuration.Users;
using System.Data.Entity;

namespace Manager.EntityFramework
{
    public class ManagerDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public ManagerDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in ManagerDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of ManagerDbContext since ABP automatically handles it.
         */
        public ManagerDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public ManagerDbContext(DbConnection connection)
            : base(connection, true)
        {

        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //Database.SetInitializer<ManagerDbContext>(null);

        //    base.OnModelCreating(modelBuilder);

        //    // TODO: comentar cuando se termina el Template
        //    //modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>("Frk");
        //}
    }
}
