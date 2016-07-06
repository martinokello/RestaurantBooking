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

namespace DataAccess.Repository
{
    public class BookingRepository : RepositoryBase<Booking, int>
    {
        public BookingRepository(RestaurantContext context):base(context)
        {
            
        }

        public override Booking FindById(int key)
        {
            return dbContext.Set<Booking>().SingleOrDefault(p => p.BookingID == key);
        }

        public override void DeleteEntity(int key)
        {
            var entity = dbContext.Set<Booking>().SingleOrDefault(p => p.BookingID == key);
            dbContext.Set<Booking>().Remove(entity);
        }

        public override void AddEntity(Booking entity)
        {
            dbContext.Set<Booking>().Add(entity);
        }

        public bool SaveBooking(Customer customer, Booking booking)
        {
            try
            {
                dbContext.Customers.Add(customer);
                dbContext.SaveChanges();
                dbContext.DinnerAttendances.Add(new DinnerAttendance
                {
                    CustomerID = customer.CustomerID,
                    Scheduled = Scheduled.NotSeated
                });
                booking.CustomerID = customer.CustomerID;
                dbContext.Bookings.Add(booking);
                return true;
            }
            catch
            {
                return false;
            }


        }
    }
}
