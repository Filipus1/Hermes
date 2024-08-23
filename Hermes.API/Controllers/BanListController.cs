using System.Text.Json;
using Hermes.Infrastructure.Dto;
using Hermes.Infrastructure.FormatSerializer;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;
[ApiController]
[Route("api/ban")]
public class BanListController : Controller
{
    private readonly IFormatSerializer _serializer;

    public BanListController(IFormatSerializer serializer)
    {
        _serializer = serializer;
    }

    [HttpGet("players")]
    public async Task<IActionResult> GetBannedPlayers()
    {
        string text = await System.IO.File.ReadAllTextAsync("/app/banlist.txt");
        var jsonBanList = _serializer.FormatToJson(text);

        return jsonBanList != null ? Ok(jsonBanList) : BadRequest();
    }

    [HttpPost("players/update")]
    public async Task<IActionResult> UpdateBannedPlayers([FromBody] List<BannedPlayersDto> dto)
    {
        string jsonString = JsonSerializer.Serialize(dto);
        string formattedString = _serializer.JsonToFormat(jsonString);

        if (formattedString == null)
        {
            return BadRequest(new { message = "Serializer has failed to parse the payload" });
        }

        await System.IO.File.WriteAllTextAsync("/app/banlist.txt", formattedString);

        return Ok(new { message = "The banlist have been updated" });
    }
}