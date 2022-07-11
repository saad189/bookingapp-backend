using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Models
{
    public class Email
    {
        [Key]
        public int Id { get; set; }

        public string Body { get; set; }

        public string ReceiverEmail { get; set; }

        public string Subject { get; set; }

        public DateTime SentTime { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
