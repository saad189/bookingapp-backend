using bookingapp_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> Get();

        Task<User> Get(int id);

        User CheckUser(string UId, string email);

        Task<User> Create(User user);

        Task Update(User user);

        Task Delete(int id);
    }
}
