using AutoMapper;
using Settlement.Api.ViewModels;
using Settlement.Application.Models;

namespace Settlement.Api.MappingProfiles;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<Booking, BookingViewModel>()
            .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BookingTime, opt => opt.MapFrom(src => new TimeOnly(src.BookingTime.Hour, src.BookingTime.Minute)));

        CreateMap<BookingViewModel, Booking>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BookingId))
            .ForMember(dest => dest.BookingTime, opt => opt.MapFrom(src => new TimeOnly(src.BookingTime.Hour, src.BookingTime.Minute)));
    }
}