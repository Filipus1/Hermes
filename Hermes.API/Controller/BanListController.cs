using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API;
[ApiController]
[Route("api/ban")]
public class BanListController : Controller
{
    public readonly IFormatSerializer serializer;

    public BanListController(IFormatSerializer serializer)
    {
        this.serializer = serializer;
    }

    [HttpGet("players")]
    public async Task<IActionResult> GetBannedPlayers()
    {
        string text = await System.IO.File.ReadAllTextAsync("/app/banlist.txt");
        var jsonBanList = serializer.FormatToJson(text);

        return jsonBanList != null ? Ok(jsonBanList) : BadRequest();
    }

    [HttpPost("players/remove")]
    public async Task<IActionResult> RemoveBannedPlayers([FromBody] List<BannedPlayerDto> dto)
    {
        string jsonString = JsonSerializer.Serialize(dto);
        string formattedString = serializer.JsonToFormat(jsonString);

        await System.IO.File.WriteAllTextAsync("/app/banlist.txt", formattedString);

        return formattedString != null ? Ok() : BadRequest();
    }
}