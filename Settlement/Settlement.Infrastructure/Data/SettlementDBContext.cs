using Microsoft.EntityFrameworkCore;

namespace Settlement.Domain.Models;

public class SettlementDBContext : DbContext
{
    public SettlementDBContext(DbContextOptions<SettlementDBContext> options) : base(options)
    {

    }

    public DbSet<Booking> Settlements { get; set; }
}