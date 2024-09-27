using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;

public interface IServerDataService
{
    public Task Add(ServerData serverData);
    public Task<IEnumerable<PlayerData>> GetRecentPlayersData();
    public Task<ServerData?> GetLatestServerData();
    public Task DeleteExpired();
}
