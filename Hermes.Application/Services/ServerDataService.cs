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

    public async Task Add(ServerData serverData)
    {
        await _repository.AddServerData(serverData);
    }

    public async Task DeleteExpired()
    {
        await _repository.DeleteExpired();
    }

    public async Task<IEnumerable<PlayerData>> GetRecentPlayersData()
    {
        return await _repository.GetRecentPlayersData();
    }

    public async Task<ServerData?> GetLatestServerData()
    {
        return await _repository.GetLatestServerData();
    }
}
