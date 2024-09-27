using System.Text.Json;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Infrastructure.Helpers;
using Quartz;

namespace Hermes.Infrastructure.CronJobs;

[DisallowConcurrentExecution]
public class ServerDataJob : IJob
{
    private readonly HttpClientSender _httpClientSender;
    private readonly IServerDataService _serverDataService;

    public ServerDataJob(HttpClientSender httpClientSender, IServerDataService serverDataService)
    {
        _httpClientSender = httpClientSender;
        _serverDataService = serverDataService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var jsonResponse = await _httpClientSender.Fetch();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var serverData = JsonSerializer.Deserialize<ServerData>(jsonResponse, options);

            if (serverData != null)
            {
                await _serverDataService.Add(serverData);
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}