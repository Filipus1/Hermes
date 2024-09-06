using Hermes.Application.Abstraction;
using Quartz;

namespace Hermes.Infrastructure.CronJobs;
[DisallowConcurrentExecution]

public class ServerHealthStatusJob : IJob
{
    private readonly HttpClient _httpClient;
    private readonly IEmailSender _emailSender;
    private static DateTime? _lastAlarmEmailSendDate;

    public ServerHealthStatusJob(HttpClient httpClient, IEmailSender emailSender)
    {
        _httpClient = httpClient;
        _emailSender = emailSender;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var monitoredServerUrl = Environment.GetEnvironmentVariable("MONITORED_SERVER_URL");

            await _httpClient.GetStringAsync(monitoredServerUrl);
        }

        catch (Exception ex)
        {
            var adminEmail = Environment.GetEnvironmentVariable("ALARM_EMAIL");

            if (_lastAlarmEmailSendDate == null || DateTime.UtcNow.Subtract(_lastAlarmEmailSendDate.Value).TotalMinutes > 30)
            {
                _lastAlarmEmailSendDate = DateTime.UtcNow;

                await _emailSender.SendEmail(adminEmail!, "Health Status Error",
                    $"<h2 style='color:red;'> {ex.Message}</h2> <br> <p style='color:black;'> StackTrace: {ex.StackTrace}</p>");
            }
        }
    }
}