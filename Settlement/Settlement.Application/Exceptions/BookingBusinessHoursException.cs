namespace Settlement.Application.Exceptions;

public class BookingBusinessHoursException : Exception
{
    public BookingBusinessHoursException() : base("Booking time is outside business hours")
    {
    }
}