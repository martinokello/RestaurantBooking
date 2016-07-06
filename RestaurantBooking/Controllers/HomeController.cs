using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Entities;
using RestaurantBooking.Models;
using System.Globalization;
using RestaurantBooking.RestaurantBusiness.Intefaces;

namespace RestaurantBooking.Controllers
{
    public class HomeController : Controller
    {
        private IRestaurantBusiness business = new RestaurantBusiness.RestaurantBusiness();

        public IRestaurantBusiness Business
        {
            set { business = value; }
            get { return business; }
        }

        [HttpGet]
        public ActionResult RestaurantBooking()
        {
            return View("RestaurantBooking");
        }
        [HttpPost]
        public ActionResult RestaurantBooking(DinnerBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dateFormat = new DateTimeFormatInfo();
                dateFormat.LongDatePattern = "dd/MM/yyyy hh:mm";
                var bookingTime = DateTime.ParseExact(model.BookingDate + " " + model.BookingTime, "dd/MM/yyyy HH:mm", dateFormat);
                var booking = new Booking { NumberOfSittings = model.NumberOfSittings, BookingTime = bookingTime };

                var customer = new Customer
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Phone = model.Phone
                    };
                var result = business.SaveBooking(customer, booking);
                if (result)
                {
                    return View("BookingCompleted");
                }
                else
                {
                    ModelState.AddModelError("FailedBooking", "The booking request has failed due to a system error");
                    return View("RestaurantBooking", model);
                }
            }
            return View("RestaurantBooking", model);
        }

        [HttpGet]
        public ActionResult MarkAttendance()
        {
            ViewBag.Attendance = InitSetUpIsSeatedUi();

            return View("MarkAttendance");
        }

        private List<List<SelectListItem>> InitSetUpIsSeatedUi()
        {
            var attendances = business.GetAllAttendances();
            ViewBag.AllAttendance = attendances;
            var selectListList = new List<List<SelectListItem>>();
            foreach (var attendanceTO in attendances)
            {
                var selectListItem1 = new SelectListItem
                    {
                        Selected = attendanceTO.IsSeated == Scheduled.Seated,
                        Text = Scheduled.Seated.ToString(),
                        Value = ((int) Scheduled.Seated).ToString()
                    };

                var selectListItem2 = new SelectListItem
                {
                    Selected = attendanceTO.IsSeated == Scheduled.NotSeated ,
                    Text = Scheduled.NotSeated.ToString(),
                    Value = ((int)Scheduled.NotSeated).ToString()
                };

                selectListList.Add(new List<SelectListItem> { selectListItem1, selectListItem2 });
            }
            return selectListList;
        }

        public ActionResult MarkAsSeated(int customerId, string isSeated)
        {
            business.MarkAsSeated(customerId, (Scheduled)Enum.Parse(typeof(Scheduled), isSeated));
            ViewBag.Attendance = InitSetUpIsSeatedUi();

            return View("MarkAttendance");
        }

        [HttpGet]
        public ActionResult StoredProcedureSelect()
        {
            return View("StoredProcedureSelect");
        }

        [HttpPost]
        public ActionResult StoredProcedureSelect(IEnumerable<Customer> customers, string customerName)
        {
            if (!string.IsNullOrEmpty(customerName))
            {
                var model = business.GetCustomerWithName(customerName);
                return View("StoredProcedureSelect", model);
            }
            return View("StoredProcedureSelect");
        }
    }
}
