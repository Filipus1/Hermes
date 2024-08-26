using Microsoft.Extensions.Options;
using Quartz;

namespace Hermes.Infrastructure.CronJobs;
public class CronJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(CronJob));
        options
            .AddJob<CronJob>(JobBuilder => JobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
            trigger.ForJob(jobKey)
            .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(5).RepeatForever())
            );
    }
}