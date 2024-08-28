using Hermes.Infrastructure.CronJobs.Manager;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;
[ApiController]
[Route("api/server")]
public class ServerMonitorController : Controller
{
    private readonly ICronJobManager _manager;
    private readonly HttpClient _client;

    public ServerMonitorController(ICronJobManager manager, HttpClient client)
    {
        _manager = manager;
        _client = client;
    }

    [HttpGet("jobs")]
    public Task<IActionResult> GetCronJobs()
    {
        var cronJobQueue = _manager.GetQueue();

        return Task.FromResult<IActionResult>(Ok(cronJobQueue));
    }

    [HttpGet("check")]
    public async Task<IActionResult> GetServerData()
    {
        try
        {
            var url = Environment.GetEnvironmentVariable("MONITORED_SERVER_URL");

            var response = await _client.GetStringAsync(url);

            return Content(response, "application/json");
        }

        catch (Exception ex)
        {
            System.Console.WriteLine($"Exception occured: {ex}");
            return BadRequest();
        }
    }
}