using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Infrastructure.Factory;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes.Infrastructure.Seed;
public static class IServiceProviderExtension
{
    public static async Task SeedToken(this IServiceProvider service)
    {
        string environment = Environment.GetEnvironmentVariable("ENVIRONMENT")!;
        if (environment == "Production")
        {
            return;
        }

        using var scope = service.CreateScope();

        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
        var factory = scope.ServiceProvider.GetRequiredService<AppContextFactory>();
        var context = factory.CreateDbContext([]);

        string token = Environment.GetEnvironmentVariable("VALIDATION_TOKEN")!;

        if (await tokenService.Get(token) != null)
        {
            return;
        }

        var validationToken = new InvitationToken
        {
            CreatedBy = "Admin",
            Token = token,
            IsUsed = false,
            ExpiryDate = DateTime.UtcNow.AddYears(100)
        };

        await context.InvitationTokens.AddAsync(validationToken);
        await context.SaveChangesAsync();
    }

    public static async Task SeedAdminUser(this IServiceProvider service)
    {
        using var scope = service.CreateScope();

        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

        await CreateAdminUser(userService, hasher);
    }

    private static async Task CreateAdminUser(IUserService userService, IPasswordHasher<User> hasher)
    {
        string adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL")!;
        string adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD")!;

        var existingAdmin = await userService.Get(adminEmail);

        if (existingAdmin != null)
        {
            return;
        }

        var adminUser = new User
        {
            Email = adminEmail,
            Password = adminPassword,
            Role = "admin"
        };

        await userService.Create(adminUser);
    }
}
