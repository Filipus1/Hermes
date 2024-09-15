namespace Hermes.Infrastructure.FileHandler;
public class FileHandler
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