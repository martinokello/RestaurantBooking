using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstracts;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Factories;
using DataAccess.Interfaces;

namespace DataAccess.Repository
{
    public class CustomerRepository : RepositoryBase<Customer, int>
    {

        public CustomerRepository(RestaurantContext context)
            : base(context)
        {

        }

        public override Customer FindById(int key)
        {
            return dbContext.Set<Customer>().SingleOrDefault(p => p.CustomerID == key);
        }

        public override void DeleteEntity(int key)
        {
            var entity = dbContext.Set<Customer>().SingleOrDefault(p => p.CustomerID == key);
            dbContext.Set<Customer>().Remove(entity);
        }

        public override void AddEntity(Customer entity)
        {
            dbContext.Set<Customer>().Add(entity);
        }
        public bool MarkAsSeated(Customer customer, Scheduled isSeated)
        {
            try
            {
                var attendance = dbContext.DinnerAttendances.SingleOrDefault(p => p.CustomerID == customer.CustomerID);
                if (attendance != null)
                    attendance.Scheduled = isSeated;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Customer GetCustomerById(int id)
        {
            return dbContext.Customers.SingleOrDefault(p => p.CustomerID == id);
        }

        public IEnumerable<Customer> GetCustomerByName(string name)
        {
            var parameter = new SqlParameter("@Name", name);
            parameter.SqlDbType = SqlDbType.VarChar;
            var results = dbContext.Database.SqlQuery<Customer>(string.Format("exec spGetAttendee @Name = '{0}'", parameter.Value));
            return results.Any() ? results.ToList() : new List<Customer>();
        }
    }
}
