using System.Text.Json;
using Hermes.Application.Entities.Dto;
using Hermes.Infrastructure.BanListFileHandler;
using Hermes.Infrastructure.BanListConverter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Hermes.API.Controllers;

[ApiController]
[Route("api/ban")]
public class BanListController : Controller
{
    private readonly BanListConverter _converter;
    private readonly BanListFileHandler _fileHandler;

    public BanListController(BanListConverter converter, BanListFileHandler fileHandler)
    {
        _converter = converter;
        _fileHandler = fileHandler;
    }

    [Authorize]
    [HttpGet("players")]
    public async Task<IActionResult> GetBannedPlayers()
    {
        var text = await _fileHandler.ReadFile();

        var jsonBanList = _converter.ParseToJson(text);

        return Ok(jsonBanList);
    }

    [Authorize]
    [HttpPost("players")]
    public async Task<IActionResult> UpdateBannedPlayers([FromBody] List<BannedPlayersDto> dto)
    {
        string jsonString = JsonSerializer.Serialize(dto);
        string formattedString = _converter.ParseToBanList(jsonString);

        await _fileHandler.WriteFile(formattedString);

        return Ok(new { message = "The banlist has been updated" });
    }
}