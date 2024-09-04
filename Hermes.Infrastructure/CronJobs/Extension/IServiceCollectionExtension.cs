using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Hermes.Infrastructure.Dto;

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

        services.AddScoped<IJob, CronJob>();

        services.ConfigureOptions<CronJobSetup>();
    }
}