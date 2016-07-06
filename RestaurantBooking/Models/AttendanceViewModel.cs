using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Entities;

namespace RestaurantBooking.Models
{
    public class AttendanceViewModel
    {
       public int CustomerID { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public Scheduled IsSeated { get; set; }
    }
}