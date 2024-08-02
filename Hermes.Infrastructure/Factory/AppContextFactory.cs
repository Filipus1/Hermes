using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class AppContextFactory : IDesignTimeDbContextFactory<AppContext>
{
    public AppContext CreateDbContext(string[] args)
    {
        var envVar = DotNetEnv.Env.Load(".env");

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        var optionsBuilder = new DbContextOptionsBuilder<AppContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new AppContext(optionsBuilder.Options);
    }
}