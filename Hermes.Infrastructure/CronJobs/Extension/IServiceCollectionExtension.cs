using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Hermes.Infrastructure.CronJobs.Extension;
public static class IServiceCollectionExtension
{
    public static void AddCronJobs(this IServiceCollection services)
    {
        services.AddQuartz();
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.AddScoped<IJob, ServerDataJob>();
        services.AddScoped<IJob, ServerHealthStatusJob>();

        services.ConfigureOptions<CronJobSetup>();
    }
}