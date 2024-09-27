using System.Globalization;

namespace Hermes.Application.Entities;

public class ServerData
{
    public int Id { get; set; }
    public string Created { get; set; } = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
    public int Players { get; set; }
    public string ServerName { get; set; } = string.Empty;
    public string GameMode { get; set; } = string.Empty;
    public bool Public { get; set; }
    public int Port { get; set; }
    public string ServerType { get; set; } = string.Empty;
    public bool HasPassword { get; set; }
    public string World { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string PreloadUrl { get; set; } = string.Empty;

    public bool IsExpired()
    {
        var createdDateTime = DateTime.ParseExact(Created, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);

        return createdDateTime.AddDays(30) < DateTime.Now;
    }
}
