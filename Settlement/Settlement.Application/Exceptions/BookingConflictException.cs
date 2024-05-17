namespace Settlement.Application.Exceptions;

public class BookingConflictException : Exception
{
    public BookingConflictException() : base("Booking time conflict")
    {
    }
}