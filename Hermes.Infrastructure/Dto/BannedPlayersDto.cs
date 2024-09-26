namespace Hermes.Infrastructure.Dto;
public record BannedPlayersDto
{
    public string Token { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
}