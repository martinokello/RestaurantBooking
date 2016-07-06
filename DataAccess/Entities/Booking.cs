using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Booking
    {
        public int BookingID { get; set; }
        public DateTime BookingTime { get; set; }
        public int NumberOfSittings { get; set; }
        public virtual int CustomerID { get; set; }
    }
}
