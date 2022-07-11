using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Models
{
    public class Lab
    {
        [Key]
        public int Id { get; set; }
        public string LabId { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }

    }
}
