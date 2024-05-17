using Microsoft.Extensions.DependencyInjection;
using Settlement.Application.Services;
using Microsoft.EntityFrameworkCore;
using Settlement.Application.Models;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDbContext<SettlementDBContext>(options => options.UseInMemoryDatabase(databaseName: "SettlementDB"));
        services.AddTransient<IBookingService, BookingService>();   
        return services;
    }
}