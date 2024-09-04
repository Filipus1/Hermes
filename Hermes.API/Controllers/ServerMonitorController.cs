using AutoMapper;
using Hermes.Application.Abstraction;
using Hermes.Infrastructure.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;
[ApiController]
[Route("api/server")]
public class ServerMonitorController : Controller
{
    private readonly HttpClient _client;
    private readonly IServerDataService _serverDataService;

    public ServerMonitorController(HttpClient client, IServerDataService serverDataService, IMapper mapper)
    {
        _client = client;
        _serverDataService = serverDataService;
    }

    [HttpGet("jobs")]
    public async Task<IActionResult> GetPlayers()
    {
        var serverDatas = await _serverDataService.Get();

        return Ok(serverDatas);
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