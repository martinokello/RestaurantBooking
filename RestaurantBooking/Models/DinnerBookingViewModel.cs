using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RestaurantBooking.Models
{
    public class DinnerBookingViewModel
    {
        [Required(ErrorMessage = "Date required")]
        [Display(Name = "Booking Date")]
        public string BookingDate { get; set; }
        [Required(ErrorMessage = "Time are required")]
        [Display(Name = "Booking Time")]
        public string BookingTime { get; set; }
        [RegularExpression(@"^([1-9][0-9]*)$", ErrorMessage = "Number of sits should be a number")]
        [Required(ErrorMessage = "Number of sittings required")]
        [Display(Name = "Number of eaters")]
        public int NumberOfSittings { get; set; }
        [Required(ErrorMessage = "First name required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email required")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",ErrorMessage = "Email is in the wrong format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number required")]
        public string Phone { get; set; }
    }
}