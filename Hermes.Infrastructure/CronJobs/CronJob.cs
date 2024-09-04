using System.Text.Json;
using AutoMapper;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Quartz;

namespace Hermes.Infrastructure.CronJobs;
[DisallowConcurrentExecution]
public class CronJob : IJob
{
    private readonly HttpClient _httpClient;
    private readonly IServerDataService _serverDataService;

    public CronJob(HttpClient httpClient, IServerDataService serverDataService)
    {
        _httpClient = httpClient;
        _serverDataService = serverDataService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var monitoredServerUrl = Environment.GetEnvironmentVariable("MONITORED_SERVER_URL");

            var jsonResponse = await _httpClient.GetStringAsync(monitoredServerUrl);

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