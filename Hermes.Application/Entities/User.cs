namespace Hermes.Application.Entities;
public class User
{
    public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "collaborator";
}
