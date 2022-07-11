using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Uid { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public Role Role { get; set; }
        public ICollection<Booking> Bookings { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
