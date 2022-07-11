using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Models
{
    public class Instructor:User
    {
        public string Password { get; set; }
    }
}
