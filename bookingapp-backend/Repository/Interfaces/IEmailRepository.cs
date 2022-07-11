using bookingapp_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Repository.Interfaces
{
    public interface IEmailRepository
    {
        Task<IEnumerable<Email>> Get();

        Task<Email> Get(int id);

        Task<Email> Create(Email email);

        Task Update(Email email);

        Task Delete(int id);
    }
}
