using Hermes.Application.Abstraction;
using Quartz;

namespace Hermes.Infrastructure.CronJobs;

[DisallowConcurrentExecution]
public class ServerDataExpiredJob : IJob
{
    private readonly IServerDataService _serverDataService;

    public ServerDataExpiredJob(IServerDataService serverDataService)
    {
        _serverDataService = serverDataService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _serverDataService.DeleteExpired();
    }
}