namespace Hermes.Application.Entities.Dto;

public record CollaboratorDto
{
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}