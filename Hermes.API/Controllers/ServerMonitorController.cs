using Hermes.Application.Abstraction;
using Hermes.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;
[ApiController]
[Route("api/server")]
public class ServerMonitorController : Controller
{
    private readonly HttpClientSender _httpClientSender;
    private readonly IServerDataService _serverDataService;

    public ServerMonitorController(HttpClientSender httpClientSender, IServerDataService serverDataService)
    {
        _httpClientSender = httpClientSender;
        _serverDataService = serverDataService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetServerData()
    {
        var data = await _serverDataService.GetLatestServerData();

        return Ok(data);
    }

    [Authorize]
    [HttpGet("players")]
    public async Task<IActionResult> GetRecentPlayers()
    {
        var data = await _serverDataService.GetRecentPlayersData();

        return Ok(data);
    }

    [Authorize]
    [HttpGet("status")]
    public async Task<IActionResult> GetServerStatus()
    {
        try
        {
            await _httpClientSender.Fetch();

            return Ok(new { message = "Online" });
        }

        catch (Exception)
        {
            return Ok(new { message = "Offline" });
        }
    }
}