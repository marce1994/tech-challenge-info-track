using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Settlement.Application.Config;
using Settlement.Application.Exceptions;
using Settlement.Application.Models;
using Settlement.Application.Services;

namespace Settlement.Test.Services;

public class BookingServiceTest
{
    private readonly TimeOnly _businessTimeFrom = new TimeOnly(9, 0, 0);
    private readonly TimeOnly _businessTimeTo = new TimeOnly(17, 0, 0);
    private readonly int _maxSimultaneousSettlements = 4;

    private readonly IOptions<BookingServiceOptions> _options;
    private SettlementDBContext _dbContext = new SettlementDBContext(new DbContextOptionsBuilder<SettlementDBContext>()
            .UseInMemoryDatabase(databaseName: "SettlementDB").Options);

    private IBookingService _bookingService;

    public BookingServiceTest()
    {
        _options = Options.Create(new BookingServiceOptions
        {
            BusinessTimeFrom = _businessTimeFrom,
            BusinessTimeTo = _businessTimeTo,
            MaxSimultaneousSettlements = _maxSimultaneousSettlements
        });

        _bookingService = new BookingService(_dbContext, _options);
    }

    [Fact]
    public async Task CreateBooking_ShouldBeAbleToBookUntil1HourBeforeClosingTime()
    {
        var bookingTime = _businessTimeTo.AddHours(-1);

        try
        {
            await _bookingService.CreateBooking(new Booking
            {
                BookingTime = bookingTime,
                Name = "Test"
            });
        }
        catch (Exception ex)
        {
            Assert.Fail($"Unexpected exception: {ex.Message}");
        }

        ClearDatabase();
    }

    [Fact]
    public async Task CreateBooking_ShouldNotBeAbleToBook59MinutesBeforeClosingTime()
    {
        var bookingTime = _businessTimeTo.AddMinutes(-59);

        try
        {
            await _bookingService.CreateBooking(new Booking
            {
                BookingTime = bookingTime,
                Name = "Test"
            });

            Assert.Fail("Expected exception was not thrown");
        }
        catch (BookingBusinessHoursException ex)
        {
            Assert.Equal("Booking time is outside business hours", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail($"Unexpected exception: {ex.Message}");
        }

        ClearDatabase();
    }

    [Theory]
    [InlineData("9:00", "9:00", "9:00", "9:00", "9:00")]
    [InlineData("9:00", "9:25", "9:45", "9:59", "9:33")]
    public async Task CreateBooking_ThrowsBookingConflictException(params string[] hours)
    {
        var timeOnlyHours = hours.Select(x => TimeOnly.Parse(x)).ToList();

        try
        {
            foreach (var h in timeOnlyHours)
            {
                await _bookingService.CreateBooking(new Booking
                {
                    BookingTime = h,
                    Name = "Test"
                });
            }

            Assert.Fail("Expected exception was not thrown");
        }
        catch (BookingConflictException ex)
        {
            Assert.Equal("Booking time conflict", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail($"Unexpected exception: {ex.Message}");
        }

        ClearDatabase();
    }

    [Theory]
    [InlineData("9:00", "9:00", "9:00", "9:00", "10:00")]
    [InlineData("9:15", "9:30", "9:45", "10:00", "10:15")]
    public async Task CreateBooking_ShouldNotThrowBookingConflictException(params string[] hours)
    {
        var timeOnlyHours = hours.Select(x => TimeOnly.Parse(x)).ToList();
        
        try
        {
            foreach (var h in timeOnlyHours)
            {
                    await _bookingService.CreateBooking(new Booking
                    {
                        BookingTime = h,
                        Name = "Test"
                    });
            }
        }
        catch (Exception ex)
        {
            Assert.Fail($"Unexpected exception: {ex.Message}");
        }

        ClearDatabase();
    }

    [Theory]
    [InlineData("John", "8:59")]
    [InlineData("Sam", "17:01")]
    [InlineData("Miguel", "16:01")]
    [InlineData("Juan", "00:00")]
    public async Task CreateBooking_ThrowsBookingBusinessHoursException(string name, string hour)
    {
        var timeOnlyHour = TimeOnly.Parse(hour);

        try
        {
            await _bookingService.CreateBooking(new Booking
            {
                BookingTime = timeOnlyHour,
                Name = name
            });

            Assert.Fail("Expected exception was not thrown");
        }
        catch (BookingBusinessHoursException ex)
        {
            Assert.Equal("Booking time is outside business hours", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail($"Unexpected exception: {ex.Message}");
        }

        ClearDatabase();
    }

    private void ClearDatabase()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
    }
}