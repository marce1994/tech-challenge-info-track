using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Settlement.Api.ViewModels;
using Settlement.Application.Exceptions;
using Settlement.Application.Models;
using Settlement.Application.Services;

namespace Settlement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController(IBookingService bookingService, IMapper mapper) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<BookingViewModel>> GetBookings()
    {
        var bookings = await _bookingService.GetBookings();
        var bookingViewModels = _mapper.Map<IEnumerable<BookingViewModel>>(bookings);
        return Ok(bookingViewModels);   
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingViewModel>> GetBooking(Guid id)
    {
        var booking = await _bookingService.GetBooking(id);
        
        if (booking == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<BookingViewModel>(booking));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(BookingViewModel booking)
    {
        try {
            var createdBooking = await _bookingService.CreateBooking(_mapper.Map<Booking>(booking));

            return Ok(new { BookingId = createdBooking.Id });
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
        try
        {
            await _bookingService.DeleteBooking(id);
        } catch (BookingNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }   
}