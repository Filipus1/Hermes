using Quartz;

namespace Hermes.Infrastructure.CronJobs;
[DisallowConcurrentExecution]
public class CronJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        return Task.CompletedTask;
    }
}