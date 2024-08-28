using System.Text.Json;
using AutoMapper;
using Hermes.Application.Entities;
using Hermes.Infrastructure.CronJobs.Manager;
using Hermes.Infrastructure.Dto;
using Quartz;

namespace Hermes.Infrastructure.CronJobs;
[DisallowConcurrentExecution]
public class CronJob : IJob
{
    private readonly HttpClient _httpClient;
    private readonly ICronJobManager _manager;
    private readonly IMapper _mapper;

    public CronJob(HttpClient httpClient, ICronJobManager manager, IMapper mapper)
    {
        _httpClient = httpClient;
        _manager = manager;
        _mapper = mapper;
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
            var dto = _mapper.Map<ServerDataDto>(serverData);

            _manager.EnqueueJob(dto);
        }

        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}