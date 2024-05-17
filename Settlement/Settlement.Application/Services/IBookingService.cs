using Settlement.Application.Models;

namespace Settlement.Application.Services;

public interface IBookingService
{
    Task<IEnumerable<Booking>> GetBookings();
    Task<Booking> GetBooking(Guid id);
    Task<Booking> CreateBooking(Booking settlement);
    Task DeleteBooking(Guid id);
}