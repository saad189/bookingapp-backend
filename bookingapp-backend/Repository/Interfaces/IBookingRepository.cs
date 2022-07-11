using bookingapp_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Repository.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> Get();

        Task<Booking> Get(int id);

        Task<IEnumerable<Booking>> Get(DateTime date);

        Task<Booking> Create(Booking booking);

        Task Update(Booking booking);

        Task Delete(int id);
    }
}
