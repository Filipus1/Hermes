namespace Hermes.Application.Entities.Dto;
public record RegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}