namespace Hermes.Application.Entities;
public record PlayerData
{
    public int Players { get; set; }
    public string Created { get; set; } = string.Empty;
}