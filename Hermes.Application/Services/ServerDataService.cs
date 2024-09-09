using Hermes.Application.Abstraction;
using Hermes.Application.Entities;

namespace Hermes.Application.Services;

public class ServerDataService : IServerDataService
{
    private readonly IServerDataRepository _repository;

    public ServerDataService(IServerDataRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Add(ServerData serverData)
    {
        return await _repository.AddServerData(serverData);
    }

    public async Task<IEnumerable<ServerData>> Get()
    {
        return await _repository.GetServerData();
    }

    public async Task DeleteExpired()
    {
        await _repository.DeleteExpired();
    }
}
