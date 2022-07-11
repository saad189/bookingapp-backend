using bookingapp_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Repository.Interfaces
{
   public interface ILabRepository
    {
        Task<IEnumerable<Lab>> Get();

        Task<Lab> Get(int id);

        Task<Lab> Create(Lab lab);

        Task Update(Lab lab);

        Task Delete(int id);
    }
}
