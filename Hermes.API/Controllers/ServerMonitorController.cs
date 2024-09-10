using Hermes.Application.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;
[ApiController]
[Route("api/server")]
public class ServerMonitorController : Controller
{
    private readonly HttpClient _client;
    private readonly IServerDataService _serverDataService;

    public ServerMonitorController(HttpClient client, IServerDataService serverDataService)
    {
        _client = client;
        _serverDataService = serverDataService;
    }

    [HttpGet("get/data")]
    public async Task<IActionResult> GetServerData()
    {
        var data = await _serverDataService.Get();

        return Ok(data);
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetServerStatus()
    {
        try
        {
            var url = Environment.GetEnvironmentVariable("MONITORED_SERVER_URL");

            var response = await _client.GetStringAsync(url);

            return Ok(new { message = "Online" });
        }

        catch (Exception)
        {
            return BadRequest(new { message = "Offline" });
        }
    }
}