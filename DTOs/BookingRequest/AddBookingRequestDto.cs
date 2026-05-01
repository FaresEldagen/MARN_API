using MARN_API.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.BookingRequest
{
    public class AddBookingRequestDto
    {
        [Required]
        public long PropertyId { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public PaymentFrequency PaymentFrequency { get; set; }
    }
}
