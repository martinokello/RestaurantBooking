using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class DinnerAttendance
    {
        public int DinnerAttendanceID { get; set; }
        public Scheduled Scheduled { get; set; }
        public virtual int CustomerID { get; set; }
    }

    public enum Scheduled{Seated,NotSeated}
}
