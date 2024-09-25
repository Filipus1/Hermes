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

    [HttpGet]
    public async Task<IActionResult> GetServerData()
    {
        var data = await _serverDataService.GetLatestServerData();

        return Ok(data);
    }

    [HttpGet("players")]
    public async Task<IActionResult> GetRecentPlayers()
    {
        var data = await _serverDataService.GetRecentPlayersData();

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
            return Ok(new { message = "Offline" });
        }
    }
}