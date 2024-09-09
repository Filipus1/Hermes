using Microsoft.Extensions.Options;
using Quartz;

namespace Hermes.Infrastructure.CronJobs;
public class CronJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var serverDataJobKey = JobKey.Create(nameof(ServerDataJob));
        options
            .AddJob<ServerDataJob>(JobBuilder => JobBuilder.WithIdentity(serverDataJobKey))
            .AddTrigger(trigger =>
            trigger.ForJob(serverDataJobKey)
            .WithSimpleSchedule(schedule => schedule.WithIntervalInHours(1).RepeatForever())
            );

        var serverHealthStatusJobKey = JobKey.Create(nameof(ServerHealthStatusJob));
        options
            .AddJob<ServerHealthStatusJob>(JobBuilder => JobBuilder.WithIdentity(serverHealthStatusJobKey))
            .AddTrigger(trigger =>
            trigger.ForJob(serverHealthStatusJobKey)
            .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10).RepeatForever())
            );

        var serverDataExpiredJobKey = JobKey.Create(nameof(ServerDataExpiredJob));
        options
            .AddJob<ServerDataExpiredJob>(JobBuilder => JobBuilder.WithIdentity(serverDataExpiredJobKey))
            .AddTrigger(trigger =>
            trigger.ForJob(serverDataExpiredJobKey)
            .WithSimpleSchedule(schedule => schedule.WithIntervalInHours(24).RepeatForever())
            );
    }
}