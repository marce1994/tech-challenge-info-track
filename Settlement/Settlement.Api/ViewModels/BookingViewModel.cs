namespace Settlement.Api.ViewModels;

public record BookingViewModel
{
    public Guid BookingId { get; set; }
    public string Name { get; set; } = "";
    public TimeOnly BookingTime { get; set; }
}