namespace Settlement.Domain.Models;

public class Booking
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public TimeOnly BookingTime { get; set; }
}