namespace Hermes.Infrastructure.FileHandler;
public class FileHandler
{
    public async Task<string> ReadFile()
    {
        return await File.ReadAllTextAsync("/app/banlist.txt");
    }

    public async Task WriteFile(string formattedString)
    {
        await File.WriteAllTextAsync("/app/banlist.txt", formattedString);
    }
}