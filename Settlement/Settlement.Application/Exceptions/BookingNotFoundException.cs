namespace Settlement.Application.Exceptions;

public class BookingNotFoundException : Exception
{
    public BookingNotFoundException() : base("Booking not found")
    {
    }
}