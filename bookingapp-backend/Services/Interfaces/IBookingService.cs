using bookingapp_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Services.Interfaces
{
   public interface IBookingService
    {
        public DateTime[] CreateBookingSlots();

        public Task<string?> IsValidBooking(Booking booking,bool isInstructor = false);
    }
}
