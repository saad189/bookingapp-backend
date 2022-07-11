using bookingapp_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Services.Interfaces
{
   public interface IEmailService
    {
       Task SendEmailAsync(Email email);

       Email CreateEmail(string receiverEmail,string receiverName, string subject, Booking booking);
    }
}
