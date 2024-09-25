using Elastic.Clients.Elasticsearch;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Infrastructure.Pagination;

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

    public async Task<PaginatedList<ElasticLog>> GetPaginated(int pageNumber, int pageSize)
    {
        try
        {
            int from = pageNumber * pageSize;
            var response = await _client.SearchAsync<ElasticLog>((s) =>
                s.Index(INDEX_NAME)
                .From(from)
                .Size(pageSize));

            var source = response.Documents.ToList();
            var totalCount = response.Total;

            PaginatedList<ElasticLog> list = new(source, pageNumber, pageSize, totalCount);

            return list;
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new PaginatedList<ElasticLog>();
        }
    }

    public async Task<PaginatedList<ElasticLog>> SearchPaginated(int pageNumber, int pageSize, string message)
    {
        try
        {
            int from = pageNumber * pageSize;

            var response = string.IsNullOrEmpty(message) ? await _client.SearchAsync<ElasticLog>(s => s
                  .Index(INDEX_NAME)
                  .From(from)
                  .Size(pageSize)
                  .Query(q => q
                      .MatchAll(m => m.QueryName("message")
                      )
                  )
               )
           :
          await _client.SearchAsync<ElasticLog>(s => s
                .Index(INDEX_NAME)
                .From(from)
                .Size(pageSize)
                .Query(q => q
                    .MatchPhrasePrefix(mpp => mpp
                    .Field("message"!)
                    .Query(message)
                    )
                )
        );

            var source = response.Documents.ToList();
            var totalCount = response.Total;

            PaginatedList<ElasticLog> list = new(source, pageNumber, pageSize, totalCount);

            return list;
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new PaginatedList<ElasticLog>();
        }
    }
}