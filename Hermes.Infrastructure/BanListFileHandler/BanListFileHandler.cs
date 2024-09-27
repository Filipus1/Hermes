namespace Hermes.Infrastructure.BanListFileHandler;

public class BanListFileHandler
{
    public async Task<string> ReadFile()
    {
        var path = Environment.GetEnvironmentVariable("DOCKER_BAN_LIST_PATH");

        return await File.ReadAllTextAsync(path!);
    }

    public async Task WriteFile(string formattedString)
    {
        var path = Environment.GetEnvironmentVariable("DOCKER_BAN_LIST_PATH");

        await File.WriteAllTextAsync(path!, formattedString);
    }
}