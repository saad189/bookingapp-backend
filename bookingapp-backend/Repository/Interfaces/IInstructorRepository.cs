using bookingapp_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Repository.Interfaces
{
    public interface IInstructorRepository
    {
        Task<IEnumerable<Instructor>> Get();

        Task<Instructor> Get(int id);

        Task<Instructor> Create(Instructor Instructor);

        Task Update(Instructor Instructor);

        Task Delete(int id);
    }
}
