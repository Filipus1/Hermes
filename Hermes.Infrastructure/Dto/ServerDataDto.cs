namespace Hermes.Infrastructure.Dto;
public class ServerDataDto
{
    public int Players { get; set; }
    public string Date { get; set; } = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
}