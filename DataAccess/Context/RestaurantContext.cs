using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DataAccess.Entities;

namespace DataAccess.Context
{
    public class RestaurantContext: DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<DinnerAttendance> DinnerAttendances { get; set; }
    }
}
