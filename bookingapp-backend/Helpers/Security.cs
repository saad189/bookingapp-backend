using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bookingapp_backend.Helpers
{
    public static class Security
    {
        public static string SecurePassword(string actualPassword,string key)
        {
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(actualPassword));
            return Convert.ToBase64String(hash);
        }
    }
}
