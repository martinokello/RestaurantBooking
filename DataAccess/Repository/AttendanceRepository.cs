using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstracts;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Factories;
using DataAccess.Interfaces;
using DataAccess.TransferObjects;

namespace DataAccess.Repository
{
    public class AttendanceRepository : RepositoryBase<DinnerAttendance, int>
    {
        public AttendanceRepository(RestaurantContext context)
            : base(context)
        {
            
        }

        public override DinnerAttendance FindById(int key)
        {
            return dbContext.Set<DinnerAttendance>().SingleOrDefault(p => p.DinnerAttendanceID == key);
        }

        public override void DeleteEntity(int key)
        {
            var entity = dbContext.Set<DinnerAttendance>().SingleOrDefault(p => p.DinnerAttendanceID == key);
            dbContext.Set<DinnerAttendance>().Remove(entity);
        }

        public override void AddEntity(DinnerAttendance entity)
        {
            dbContext.Set<DinnerAttendance>().Add(entity);
        }
        public IEnumerable<AttendanceTO> GetAllAttendances()
        {
            var attendances = (from a in dbContext.DinnerAttendances
                               from c in dbContext.Customers
                               where a.CustomerID == c.CustomerID
                               select new AttendanceTO
                               {
                                   CustomerID = c.CustomerID,
                                   FirstName = c.FirstName,
                                   LastName = c.LastName,
                                   IsSeated = a.Scheduled
                               });
            return attendances;
        }
    }
}
