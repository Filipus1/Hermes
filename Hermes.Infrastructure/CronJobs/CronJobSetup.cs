using Microsoft.Extensions.Options;
using Quartz;

namespace Hermes.Infrastructure.CronJobs;
public class CronJobSetup : IConfigureOptions<QuartzOptions>
{
    private const int INTERVAL_TIME = 1;
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(CronJob));
        options
            .AddJob<CronJob>(JobBuilder => JobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
            trigger.ForJob(jobKey)
            .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(INTERVAL_TIME).RepeatForever())
            );
    }
}