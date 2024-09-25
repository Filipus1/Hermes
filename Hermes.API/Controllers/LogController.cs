using Hermes.Application.Abstraction;
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
    [HttpGet("{key}")]
    public async Task<IActionResult> GetLog(string key)
    {
        var log = await _elasticService.Get(key);

        return Ok(log);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPaginatedLogs([FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 20)
    {
        var logs = await _elasticService.GetPaginated(pageNumber, pageSize);

        return Ok(new { logs, logs.CurrentPage, logs.PageSize, logs.TotalCount, logs.TotalPages, logs.HasPreviousPage, logs.HasNextPage });
    }

    [Authorize]
    [HttpGet("search")]
    public async Task<IActionResult> SearchPaginatedLogs([FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 20, [FromQuery] string message = "")
    {
        var logs = await _elasticService.SearchPaginated(pageNumber, pageSize, message);

        return Ok(new { logs, logs.CurrentPage, logs.PageSize, logs.TotalCount, logs.TotalPages, logs.HasPreviousPage, logs.HasNextPage });
    }
}