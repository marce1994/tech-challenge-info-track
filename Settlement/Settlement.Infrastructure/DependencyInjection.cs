using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Settlement.Domain.Models;
using Microsoft.EntityFrameworkCore;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SettlementDBContext>(options => options.UseInMemoryDatabase(databaseName: "SettlementDB"));
        return services;
    }
}