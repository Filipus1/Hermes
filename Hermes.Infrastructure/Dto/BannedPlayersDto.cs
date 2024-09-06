namespace Hermes.Infrastructure.Dto;
public record BannedPlayersDto
{
    public string? Token { get; set; }
    public string? Ip { get; set; }
}