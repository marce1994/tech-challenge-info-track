using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Settlement.Application.Config;
using Settlement.Application.Exceptions;
using Settlement.Application.Models;

namespace Settlement.Application.Services;

public class BookingService(SettlementDBContext dbContext, IOptions<BookingServiceOptions> options) : IBookingService
{
    private readonly BookingServiceOptions _config = options.Value;
    private readonly SettlementDBContext _dbContext = dbContext;

    public async Task<IEnumerable<Booking>> GetBookings()
    {
        return await _dbContext.Bookings
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Booking> GetBooking(Guid id)
    {
        var booking = await _dbContext.Bookings
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id);

        if (booking is null)
        {
            throw new BookingNotFoundException();
        }

        return booking;
    }

    public Task<Booking> CreateBooking(Booking settlement)
    {
        TimeOnly bookingTime = settlement.BookingTime;

        CheckConflict(bookingTime);
        CheckBusinessHours(bookingTime);

        settlement.Id = Guid.NewGuid();
        _dbContext.AddAsync(settlement);
        _dbContext.SaveChangesAsync();

        return Task.FromResult(settlement);
    }

    public Task DeleteBooking(Guid id)
    {
        var booking = _dbContext.Bookings.SingleOrDefault(x => x.Id == id);

        if (booking is null)
        {
            throw new BookingNotFoundException();
        }

        _dbContext.Remove(booking);
        _dbContext.SaveChangesAsync();

        return Task.CompletedTask;
    }

    private void CheckConflict(TimeOnly bookingTime)
    {
        TimeOnly bookingEndTime = bookingTime.AddHours(1);

        var simultaneousBookings = _dbContext.Bookings
            .AsNoTracking()
            .Where(s =>
                bookingTime >= s.BookingTime && bookingTime < s.BookingTime.AddHours(1) ||
                bookingEndTime > s.BookingTime && bookingEndTime <= s.BookingTime.AddHours(1) ||
                bookingTime <= s.BookingTime && bookingEndTime >= s.BookingTime.AddHours(1)
            );

        if (simultaneousBookings.Count() >= _config.MaxSimultaneousSettlements)
        {
            throw new BookingConflictException();
        }
    }

    private void CheckBusinessHours(TimeOnly bookingTime)
    {
        if (bookingTime < _config.BusinessTimeFrom || bookingTime > _config.BusinessTimeTo.AddHours(-1))
        {
            throw new BookingBusinessHoursException();
        }
    }
}