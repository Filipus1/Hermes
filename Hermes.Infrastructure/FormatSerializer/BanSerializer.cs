using System.Text.Json;

public class BanSerializer : IFormatSerializer
{
    public string FormatToJson(string input)
    {
        string[] parts = input.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

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

        var result = new
        {
            players = tokenIpPairs
        };

        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }

    public string JsonToFormat(string input)
    {
        var tokenIpPairs = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(input);

        if (tokenIpPairs == null) return "";

        List<string> formattedParts = new List<string>();

        foreach (var pair in tokenIpPairs)
        {
            if (pair.ContainsKey("Token") && pair.ContainsKey("Ip"))
            {
                formattedParts.Add(pair["Token"]);
                formattedParts.Add(pair["Ip"]);
            }
        }

        return string.Join("|", formattedParts) + "|";
    }
}

