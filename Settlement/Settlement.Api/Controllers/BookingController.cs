using Microsoft.AspNetCore.Mvc;
using Settlement.Api.ViewModels;
using Settlement.Application.Exceptions;
using Settlement.Application.Services;

namespace Settlement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;

    [HttpGet]
    public async Task<ActionResult<BookingViewModel>> GetBookings()
    {
        var bookings = await _bookingService.GetBookings();
        return Ok(bookings.Select(x => new BookingViewModel
        {
            BookingId = x.Id,
            Name = x.Name,
            BookingTime = x.BookingTime
        }));   
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingViewModel>> GetBooking(Guid id)
    {
        var booking = await _bookingService.GetBooking(id);
        
        if (booking == null)
        {
            return NotFound();
        }

        return Ok(new BookingViewModel
        {
            BookingId = booking.Id,
            Name = booking.Name,
            BookingTime = booking.BookingTime
        });
    }

    [HttpPost]
    public async Task<ActionResult<BookingViewModel>> CreateBooking(BookingViewModel booking)
    {
        try {
            var createdBooking = await _bookingService.CreateBooking(new Booking
            {
                Name = booking.Name,
                BookingTime = booking.BookingTime
            });

            return Ok(new BookingViewModel
            {
                BookingId = createdBooking.Id,
                Name = createdBooking.Name,
                BookingTime = createdBooking.BookingTime
            });
        } catch (BookingConflictException)
        {
            return BadRequest("Booking time conflict");
        } catch (BookingBusinessHoursException)
        {
            return BadRequest("Booking outside business hours");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBooking(Guid id)
    {
            await _bookingService.DeleteBooking(id);
            return NoContent();
    }   
}