using Hermes.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hermes.Infrastructure.Factory;
public class AppContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        string connectionString = GetConnectionString();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }

    private string GetConnectionString()
    {
        var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");

        if (environment == "Production")
        {
            return Environment.GetEnvironmentVariable("CONNECTION_STRING")!;
        }
        else
        {
            return Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING")!;
        }
    }
}