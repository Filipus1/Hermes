using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;
public interface IElasticRepository
{
    Task<ElasticLog?> GetByKey(string key);
    Task<IEnumerable<ElasticLog?>> GetPaginated(int pageNumber, int pageSize);
}