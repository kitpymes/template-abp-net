using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Manager.EntityFramework.Repositories
{
    public abstract class ManagerRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<ManagerDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected ManagerRepositoryBase(IDbContextProvider<ManagerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class ManagerRepositoryBase<TEntity> : ManagerRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected ManagerRepositoryBase(IDbContextProvider<ManagerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
