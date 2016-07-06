using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Repository.Interfaces;
using UnitOfWorkAdapter.Interfaces;

namespace UnitOfWorkAdapter
{
    public class UnitOfWork : IUnitOfWork
    {
        private RestaurantContext context;

        public UnitOfWork(IRepository repository, RestaurantContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
