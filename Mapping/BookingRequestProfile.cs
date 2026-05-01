using AutoMapper;
using MARN_API.DTOs.BookingRequest;
using MARN_API.Models;

namespace MARN_API.Mapping
{
    public class BookingRequestProfile : Profile
    {
        public BookingRequestProfile()
        {
            CreateMap<AddBookingRequestDto, BookingRequest>();
        }
    }
}
