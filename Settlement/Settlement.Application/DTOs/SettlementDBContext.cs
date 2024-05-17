using Microsoft.EntityFrameworkCore;

public class SettlementDBContext : DbContext
{
    public SettlementDBContext(DbContextOptions<SettlementDBContext> options) : base(options)
    {

    }

    public DbSet<Booking> Bookings { get; set; }
}