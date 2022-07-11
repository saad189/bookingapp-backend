using bookingapp_backend.Models;
using bookingapp_backend.Repository.Interfaces;
using bookingapp_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Constants;

namespace bookingapp_backend.Services.Implementations
{
    public class BookingService : IBookingService
    {

        IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;

        }
        public DateTime[] CreateBookingSlots()
        {
            throw new NotImplementedException();
        }

        public async Task<string?> IsValidBooking(Booking currentBooking, bool isInstructor = false)
        {

            if (currentBooking.StartTime >= currentBooking.EndTime)
            {
                return "StartTime must be less than EndTime";
            }

            if (currentBooking.StartTime < DateTime.UtcNow)
            {
                var errorMessage = currentBooking.Id > 0 ? "Booking must be updated to a future Date and Time" : "A booking cannot be created in the past!";
                return errorMessage;
            }

            var bookingHours = (currentBooking.EndTime - currentBooking.StartTime).TotalHours;

            if (bookingHours > BookingConstants.BookingLimit)
            {
                return $"A booking can only be created with a maximum of {BookingConstants.BookingLimit} hours.";
            }

            var allBookings = await _bookingRepository.Get(currentBooking.StartTime);

            var currentUserBookings = allBookings.Where(booking => booking.Uid == currentBooking.Uid);

            var totalHours = bookingHours;

            currentUserBookings.ToList().ForEach(booking => totalHours += ((int)(booking.EndTime - booking.StartTime).TotalHours));

            if (totalHours > BookingConstants.BookingLimit && !isInstructor)
            {
                var allowedHours = BookingConstants.BookingLimit - (totalHours - bookingHours);
                var errorMessage = $"You have exceeded the total number of allowed Booking Hours ({BookingConstants.BookingLimit} hours).";
                errorMessage = allowedHours > 0 ? errorMessage + $" For today, you can make a booking of {allowedHours} hours" : errorMessage;
                return errorMessage;
            }

            var bookingLimitReached = currentUserBookings.ToList().Count >= BookingConstants.BookingLimit;

            if (bookingLimitReached && !isInstructor)
            {
                return "Maximum Booking Limit Reached for Today!";
            }

            var isBookingOverlapping = allBookings.ToList().Any(booking => booking.Id != currentBooking.Id && IsBookingOverlapped(currentBooking.StartTime, currentBooking.EndTime, booking.StartTime, booking.EndTime));

            if (isBookingOverlapping)
            {
                return $"Booking with Range {currentBooking.StartTime.ToLocalTime()} - {currentBooking.EndTime.ToLocalTime()} overlaps with an existing Booking.";
            }
         
            return null;

        }

        private bool IsBookingOverlapped(DateTime StartTimeFirst, DateTime EndTimeFirst, DateTime StartTimeSecond, DateTime EndTimeSecond)
        {
            return (StartTimeFirst < EndTimeSecond) && (StartTimeSecond < EndTimeFirst);
        }
    }
}
