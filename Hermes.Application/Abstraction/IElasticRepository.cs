using Hermes.Application.Entities;
using Hermes.Infrastructure.Pagination;

namespace Hermes.Application.Abstraction;
public interface IElasticRepository
{
    Task<ElasticLog?> GetByKey(string key);
    Task<PaginatedList<ElasticLog>> GetPaginated(int pageNumber, int pageSize);
    Task<PaginatedList<ElasticLog>> SearchPaginated(int pageNumber, int pageSize, string message);
}