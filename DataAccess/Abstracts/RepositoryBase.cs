using System.Data.Entity;
using System.Linq;
using DataAccess.Context;
using DataAccess.Interfaces;
using DataAccess.Repository.Interfaces;

namespace DataAccess.Abstracts
{
    public abstract class RepositoryBase<T, EntityKey> : IRepository where T : class
    {
        protected RestaurantContext dbContext;

        public RepositoryBase()
        {
        }

        public RepositoryBase(RestaurantContext context)
        {
            this.dbContext = context;
        }

        public IQueryable FindAll()
        {
            return dbContext.Set<T>();
        }

        public abstract T FindById(EntityKey key);

        public abstract void DeleteEntity(EntityKey key);

        public abstract void AddEntity(T entity);

    }
}
