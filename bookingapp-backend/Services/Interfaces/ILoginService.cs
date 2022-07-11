using bookingapp_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<Instructor> CheckLogin(string Uid,string password);

        public string GenerateJwtToken(Instructor instructor);
    }
}
