using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;
[ApiController]
[Route("api/log")]
public class LogController : Controller
{
    private readonly IElasticService _elasticService;

    public LogController(IElasticService elasticService)
    {
        _elasticService = elasticService;
    }

    [Authorize]
    [HttpGet("get-log/{key}")]
    public async Task<IActionResult> GetLog(string key)
    {
        var log = await _elasticService.Get(key);

        return Ok(log);
    }

    [Authorize]
    [HttpGet("get-logs/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetPaginatedLogs(int pageNumber, int pageSize)
    {
        var logs = await _elasticService.GetPaginated(pageNumber, pageSize);

        return Ok(logs);
    }
}