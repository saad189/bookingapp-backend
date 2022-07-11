using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.DTOs
{
    public class JwtModel
    {
        public string idToken { get; set; }
        public DateTime expiresAt { get; set; }
    }
}
