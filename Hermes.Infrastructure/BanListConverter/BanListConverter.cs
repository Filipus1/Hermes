using System.Text.Json;

namespace Hermes.Infrastructure.BanListConverter;
public class BanListConverter
{
    public string ParseToJson(string banList)
    {
        string[] parts = banList.Split('|', StringSplitOptions.RemoveEmptyEntries);

        var tokenIpPairs = new List<Dictionary<string, string>>();

        for (int i = 0; i < parts.Length; i += 2)
        {
            if (i + 1 < parts.Length)
            {
                string token = parts[i];
                string ip = parts[i + 1];

                var tokenIp = new Dictionary<string, string>
                {
                    { "Token", token },
                    { "Ip", ip }
                };

                tokenIpPairs.Add(tokenIp);
            }
        }

        return JsonSerializer.Serialize(tokenIpPairs);
    }

    public string ParseToBanList(string jsonInput)
    {
        var tokenIpPairs = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(jsonInput);

        if (tokenIpPairs == null)
        {
            throw new InvalidOperationException("Deserialization failed. The input JSON was invalid or incorrectly formatted.");
        }

        List<string> formattedParts = new List<string>();

        foreach (var pair in tokenIpPairs)
        {
            if (pair.TryGetValue("Token", out string? token) && pair.TryGetValue("Ip", out string? Ip))
            {
                formattedParts.Add(token);
                formattedParts.Add(Ip);
            }
        }

        return string.Join("|", formattedParts) + "|";
    }
}

