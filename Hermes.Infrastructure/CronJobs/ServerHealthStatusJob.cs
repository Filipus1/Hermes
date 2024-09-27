using Hermes.Application.Abstraction;
using Hermes.Infrastructure.Helpers;
using Quartz;

namespace Hermes.Infrastructure.CronJobs;

[DisallowConcurrentExecution]
public class ServerHealthStatusJob : IJob
{
    private readonly HttpClientSender _httpClientSender;
    private readonly IEmailSender _emailSender;
    private static DateTime? _lastAlarmEmailSendDate;

    public ServerHealthStatusJob(IEmailSender emailSender, HttpClientSender httpClientSender)
    {
        _emailSender = emailSender;
        _httpClientSender = httpClientSender;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await _httpClientSender.Fetch();
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