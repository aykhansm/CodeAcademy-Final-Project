using shitfo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class Booking:BaseEntity
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public BookingStatus Status { get; set; }
        
        public int Amount { get; set; }
        [Required]
        public DateTime BookingStart { get; set; }
        [Required]
        public int BookingDuration { get; set; }
        
        public DateTime BookingEnd { get; set; }
    }
}
