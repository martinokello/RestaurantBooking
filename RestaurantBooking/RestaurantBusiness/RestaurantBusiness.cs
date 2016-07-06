using System.Collections.Generic;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using DataAccess.TransferObjects;
using RestaurantBooking.RestaurantBusiness.Intefaces;
using UnitOfWorkAdapter;
using UnitOfWorkAdapter.Interfaces;

namespace RestaurantBooking.RestaurantBusiness
{
    public class RestaurantBusiness:IRestaurantBusiness
    {
        private RestaurantContext contextAttendance = new RestaurantContext();
        private RestaurantContext contextCustomer = new RestaurantContext();
        private RestaurantContext contextBooking = new RestaurantContext();
        private IRepository attRepository;
        private IRepository custRepository;
        private IRepository bookingRepository;
        private IUnitOfWork attUnitOfWork;
        private IUnitOfWork custUnitOfWork;
        private IUnitOfWork bookingUnitOfWork;

        public RestaurantBusiness()
        {
            attRepository = new AttendanceRepository(contextAttendance);
            custRepository = new CustomerRepository(contextCustomer);
            bookingRepository = new BookingRepository(contextBooking);
            attUnitOfWork = new UnitOfWork(attRepository, contextAttendance);
            custUnitOfWork = new UnitOfWork(custRepository, contextCustomer);
            bookingUnitOfWork = new UnitOfWork(bookingRepository, contextBooking);
        }

        public bool SaveBooking(Customer customer, Booking booking)
        {
            var retVal = (bookingRepository as BookingRepository).SaveBooking(customer, booking);
            bookingUnitOfWork.SaveChanges();
            return retVal;

        }
        public bool MarkAsSeated(int customerId, Scheduled isSeated)
        {
            var retVal = (custRepository as CustomerRepository).MarkAsSeated((custRepository as CustomerRepository).GetCustomerById(customerId), isSeated);
            custUnitOfWork.SaveChanges();
            return retVal;

        }

        public IEnumerable<AttendanceTO> GetAllAttendances()
        {
            return (attRepository as AttendanceRepository).GetAllAttendances();
        }

        public IEnumerable<Customer> GetCustomerWithName(string name)
        {
            return (custRepository as CustomerRepository).GetCustomerByName(name);
        }
    }
}