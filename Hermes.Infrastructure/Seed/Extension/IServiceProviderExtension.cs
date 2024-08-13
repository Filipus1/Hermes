using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes.Infrastructure.Seed;
public static class IServiceProviderExtension
{
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

        if (existingAdmin != null) return;

        var adminUser = new User
        {
            Email = adminEmail,
            Password = adminPassword,
            Role = "admin"
        };

        await userService.Create(adminUser);
    }
}
