using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.DTOs
{
    public class BookingEvent
    {
        public int Id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string title { get; set; }

        public string email { get; set; }

        public int labId { get; set; }
        public string uid { get; set; }

    }
}
