using Elastic.Clients.Elasticsearch;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities;

namespace Hermes.Infrastructure.Repositories;
public class ElasticRepository : IElasticRepository
{
    private const string INDEX_NAME = "npm-logs";
    private readonly ElasticsearchClient _client;

    public ElasticRepository()
    {
        var url = Environment.GetEnvironmentVariable("ELASTICSEARCH_URL");
        var settings = new ElasticsearchClientSettings(new Uri(url!))
            .DefaultIndex(INDEX_NAME);

        _client = new ElasticsearchClient(settings);
    }

    public async Task<ElasticLog?> GetByKey(string key)
    {
        var response = await _client.GetAsync<ElasticLog>(key, (g) =>
        {
            g.Index(INDEX_NAME);
        });

        return response.Source;
    }

    public async Task<IEnumerable<ElasticLog?>> GetPaginated(int pageNumber, int pageSize)
    {
        try
        {
            int from = (pageNumber - 1) * pageSize;
            var response = await _client.SearchAsync<ElasticLog>((g) =>
            g.Index(INDEX_NAME)
            .From(from)
            .Size(pageSize)
            );

            return response.Documents.ToList();
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new List<ElasticLog?>();
        }
    }
}