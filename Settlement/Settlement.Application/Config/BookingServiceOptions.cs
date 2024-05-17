namespace Settlement.Application.Config
{
    public class BookingServiceOptions
    {
        public const string BookingService = "BookingService";
        public TimeOnly BusinessTimeFrom { get; set; }
        public TimeOnly BusinessTimeTo { get; set; }
        public int MaxSimultaneousSettlements { get; set; }
    }
}
