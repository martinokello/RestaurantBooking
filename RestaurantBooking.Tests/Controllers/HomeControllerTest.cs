using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DataAccess;
using DataAccess.Abstracts;
using DataAccess.Context;
using DataAccess.Repository;
using Moq;
using DataAccess.Entities;
using DataAccess.Factories;
using DataAccess.Interfaces;
using DataAccess.TransferObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantBooking.Controllers;
using RestaurantBooking.Models;
using RestaurantBooking.RestaurantBusiness.Intefaces;
using UnitOfWorkAdapter.Interfaces;

namespace RestaurantBooking.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private RepositoryBase<Customer, int> custRepository = null;
        private RepositoryBase<DinnerAttendance, int> attRepository = null;
        private RepositoryBase<Booking, int> bookingRepository = null;
        private IRestaurantBusiness business = null;
        private Customer customer = null;
        private Booking booking = null;
        private Mock<IRestaurantBusiness> mock;

        [TestInitialize]
        public void SetUp()
        {
            var guidNewUserEmail = Guid.NewGuid() + "@gmail.com";
            var customerId = (new Random().Next(DateTime.Now.Millisecond));
            customer = new Customer
                {
                    CustomerID = customerId,
                    Email = guidNewUserEmail,
                    FirstName = "Martin",
                    LastName = "Okello",
                    Phone = "03939399393"
                };
            booking = new Booking { BookingTime = DateTime.Now, BookingID = customerId, CustomerID = customerId, NumberOfSittings = 2 };

            custRepository = new FakeCustomerRepository();
            attRepository = new FakeAttendanceRepository();
            bookingRepository = new FakeBookingRepository();

            mock = new Mock<IRestaurantBusiness>();
            mock.Setup(p => p.GetAllAttendances()).Returns((attRepository as FakeAttendanceRepository).GetAllAttendances());
            mock.Setup(p => p.MarkAsSeated(1, Scheduled.NotSeated)).Returns((custRepository as FakeCustomerRepository).MarkAsSeated(customer, Scheduled.NotSeated));
            mock.Setup(p => p.MarkAsSeated(1, Scheduled.NotSeated)).Verifiable();
            mock.Setup(p => p.SaveBooking(It.IsAny<Customer>(), It.IsAny<Booking>())).Returns(true);
            mock.Setup(p => p.SaveBooking(customer, booking)).Verifiable();
            business = mock.Object;

        }

        [TestMethod]
        public void RestaurantBookingReturnsProperView()
        {
            // Arrange
            var controller = new HomeController();

            controller.Business = business;
            // Act
            ViewResult result = controller.RestaurantBooking() as ViewResult;

            // Assert
            Assert.AreEqual(result.ViewName, "RestaurantBooking");
        }
        [TestMethod]
        public void SaveBookingReturnsCorrectView()
        {
            //Arrange
            var controller = new HomeController();
            //
            controller.Business = business;
            var guidNewUserEmail = Guid.NewGuid() + "@gmail.com";
            var model = new DinnerBookingViewModel
                {
                    BookingDate = "14/11/2013",
                    BookingTime = "12:30",
                    Email = guidNewUserEmail,
                    FirstName = "Martin",
                    LastName = "Okello",
                    Phone = "07809773365",
                    NumberOfSittings = 5
                };
            //Act
            ViewResult result = (ViewResult)controller.RestaurantBooking(model);

            //Assert
            Assert.AreEqual(result.ViewName, "BookingCompleted");

        }
        [TestMethod]
        public void MarkAttendanceReturnsProperView()
        {
            // Arrange
            var controller = new HomeController();

            controller.Business = business;
            // Act
            ViewResult result = controller.MarkAttendance() as ViewResult;

            // Assert
            Assert.AreEqual(result.ViewName, "MarkAttendance");
        }


        [TestMethod]
        public void MarkAttendanceIsSeated()
        {
            //Arrange
            var controller = new HomeController();

            controller.Business = business;

            //Act
            ViewResult result = (ViewResult)controller.MarkAsSeated(1, Scheduled.NotSeated.ToString());

            //Assert
            Assert.AreEqual(result.ViewName, "MarkAttendance");

        }

        [TestMethod]
        public void test_mark_as_seated_is_called()
        {
            //Arrange
            var controller = new HomeController();

            controller.Business = business;

            //Act

            ViewResult result = (ViewResult)controller.MarkAsSeated(1, Scheduled.NotSeated.ToString());

            //Assert
            mock.Verify(p => p.MarkAsSeated(It.IsAny<int>(), It.IsAny<Scheduled>()), Times.AtLeastOnce);

        }

    }
    public class FakeUnityOfWork : IUnitOfWork
    {
        public void SaveChanges()
        {
        }
    }

    public class FakeCustomerRepository : RepositoryBase<Customer, int>
    {
        public FakeCustomerRepository()
        {

        }

        public IEnumerable<AttendanceTO> GetAllAttendances()
        {
            return new List<AttendanceTO>
                    {
                        new AttendanceTO{CustomerID = 1,FirstName = "Martin",LastName = "Okello",IsSeated = Scheduled.NotSeated},
                        new AttendanceTO{CustomerID = 2,FirstName = "James",LastName = "Macallan",IsSeated = Scheduled.Seated},
                        new AttendanceTO{CustomerID = 1,FirstName = "Mike",LastName = "Leborne",IsSeated = Scheduled.NotSeated}
                    };
        }

        public Customer GetCustomerById(int id)
        {
            var customer = GetAllAttendances().SingleOrDefault(p => p.CustomerID == id);
            return new Customer
            {
                CustomerID = customer.CustomerID,
                Email = "martin.okello@gmail.com",
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = "07809773365"
            };
        }

        public IEnumerable<Customer> GetCustomerByName(string name)
        {
            var customer = GetAllAttendances().SingleOrDefault(p => p.FirstName == name);
            return new List<Customer>
                {
                    new Customer
                        {
                            CustomerID = customer.CustomerID,
                            Email = "martin.okello@gmail.com",
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            Phone = "07809773365"
                        }
                };
        }

        public override Customer FindById(int key)
        {
            throw new NotImplementedException();
        }

        public override void DeleteEntity(int key)
        {
            throw new NotImplementedException();
        }

        public override void AddEntity(Customer entity)
        {
            throw new NotImplementedException();
        }

        public bool MarkAsSeated(Customer customer, Scheduled notSeated)
        {
            return false;
        }
    }
    public class FakeBookingRepository : RepositoryBase<Booking, int>
    {
        public FakeBookingRepository()
        {

        }

        public bool SaveBooking(Customer customer, Booking booking)
        {
            return true;
        }

        public override void DeleteEntity(int key)
        {
        }

        public override void AddEntity(Booking entity)
        {
        }

        public override Booking FindById(int key)
        {
            return new Booking();
        }
    }
    public class FakeAttendanceRepository : RepositoryBase<DinnerAttendance, int>
    {
        public FakeAttendanceRepository()
        {

        }

        public bool SaveBooking(Customer customer, Booking booking)
        {
            return true;
        }


        public IEnumerable<AttendanceTO> GetAllAttendances()
        {
            return new List<AttendanceTO>
                    {
                        new AttendanceTO{CustomerID = 1,FirstName = "Martin",LastName = "Okello",IsSeated = Scheduled.NotSeated},
                        new AttendanceTO{CustomerID = 2,FirstName = "James",LastName = "Macallan",IsSeated = Scheduled.Seated},
                        new AttendanceTO{CustomerID = 1,FirstName = "Mike",LastName = "Leborne",IsSeated = Scheduled.NotSeated}
                    };
        }

        public Customer GetCustomerById(int id)
        {
            var customer = GetAllAttendances().SingleOrDefault(p => p.CustomerID == id);
            return new Customer
            {
                CustomerID = customer.CustomerID,
                Email = "martin.okello@gmail.com",
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = "07809773365"
            };
        }
 
        public IEnumerable<Customer> GetCustomerByName(string name)
        {
            var customer = GetAllAttendances().SingleOrDefault(p => p.FirstName == name);
            return new List<Customer>
                {
                    new Customer
                        {
                            CustomerID = customer.CustomerID,
                            Email = "martin.okello@gmail.com",
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            Phone = "07809773365"
                        }
                };
        }

        public override DinnerAttendance FindById(int key)
        {
            return new DinnerAttendance();
        }

        public override void DeleteEntity(int key)
        {
        }

        public override void AddEntity(DinnerAttendance entity)
        {
        }
    }
}
