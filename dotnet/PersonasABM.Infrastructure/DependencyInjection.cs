using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonasABM.Application.Interfaces;
using PersonasABM.Infrastructure.Data;
using PersonasABM.Infrastructure.Repositories;

namespace PersonasABM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Server=localhost;Port=3306;Database=PersonasABM;Uid=personas_user;Pwd=personas123;";

        Console.WriteLine($"Connection String: {connectionString}");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            if (connectionString.Contains("Data Source"))
            {
                Console.WriteLine("Using SQLite");
                options.UseSqlite(connectionString);
            }
            else if (connectionString.Contains("Port=3306") || connectionString.Contains("Uid="))
            {
                Console.WriteLine("Using MySQL");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
            else
            {
                Console.WriteLine("Using SQL Server");
                options.UseSqlServer(connectionString);
            }
        });

        services.AddScoped<IPersonaRepository, PersonaRepository>();

        return services;
    }
}
