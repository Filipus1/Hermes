using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Hermes.Infrastructure.Factory;

namespace Hermes.Infrastructure.Context.Extension
{
    public static class IApplicationBuilderExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app, string[] args)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var factory = scope.ServiceProvider.GetRequiredService<AppContextFactory>();
            var context = factory.CreateDbContext(args);

            context.Database.Migrate();
        }
    }
}
