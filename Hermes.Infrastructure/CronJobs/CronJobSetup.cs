using Microsoft.Extensions.Options;
using Quartz;

namespace Hermes.Infrastructure.CronJobs;

public class CronJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        ConfigureJob<ServerDataJob>(options, nameof(ServerDataJob), TimeSpan.FromHours(1));
        ConfigureJob<ServerHealthStatusJob>(options, nameof(ServerHealthStatusJob), TimeSpan.FromMinutes(1));
        ConfigureJob<ServerDataExpiredJob>(options, nameof(ServerDataExpiredJob), TimeSpan.FromHours(24));
    }

    private void ConfigureJob<TJob>(QuartzOptions options, string jobName, TimeSpan interval) where TJob : IJob
    {
        var jobKey = JobKey.Create(jobName);

        options.AddJob<TJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
               .AddTrigger(trigger =>
                   trigger.ForJob(jobKey)
                          .WithSimpleSchedule(schedule =>
                              schedule.WithInterval(interval).RepeatForever()));
    }
}
