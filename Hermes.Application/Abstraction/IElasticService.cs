using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;
public interface IElasticService
{
    Task<ElasticLog?> Get(string key);
    Task<IEnumerable<ElasticLog?>> GetPaginated(int pageNumber, int pageSize);
}