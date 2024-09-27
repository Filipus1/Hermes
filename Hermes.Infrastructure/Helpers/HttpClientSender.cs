namespace Hermes.Infrastructure.Helpers;

public class HttpClientSender
{
    private readonly HttpClient _client;

    public HttpClientSender(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> Fetch()
    {
        var url = Environment.GetEnvironmentVariable("MONITORED_SERVER_URL");

        return await _client.GetStringAsync(url);
    }
}
