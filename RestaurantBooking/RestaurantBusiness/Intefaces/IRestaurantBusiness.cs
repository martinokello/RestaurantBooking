using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Entities;
using DataAccess.TransferObjects;

namespace RestaurantBooking.RestaurantBusiness.Intefaces
{
    public interface IRestaurantBusiness
    {
        bool SaveBooking(Customer customer, Booking booking);
        bool MarkAsSeated(int customerId, Scheduled isSeated);
        IEnumerable<AttendanceTO> GetAllAttendances();
        IEnumerable<Customer> GetCustomerWithName(string name);
    }
}