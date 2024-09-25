using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;
public interface IServerDataRepository
{
    public Task AddServerData(ServerData serverData);
    public Task<IEnumerable<PlayerData>> GetRecentPlayersData();
    public Task<ServerData?> GetLatestServerData();
    public Task DeleteExpired();
}
