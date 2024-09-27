using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Infrastructure.Pagination;

namespace Hermes.Application.Services;

public class ElasticService : IElasticService
{
    private readonly IElasticRepository _repository;

    public ElasticService(IElasticRepository repository)
    {
        _repository = repository;
    }

    public async Task<ElasticLog?> Get(string key)
    {
        return await _repository.GetByKey(key);
    }

    public async Task<PaginatedList<ElasticLog>> GetPaginated(int pageNumber, int pageSize)
    {
        return await _repository.GetPaginated(pageNumber, pageSize);
    }

    public async Task<PaginatedList<ElasticLog>> SearchPaginated(int pageNumber, int pageSize, string message)
    {
        return await _repository.SearchPaginated(pageNumber, pageSize, message);
    }
}