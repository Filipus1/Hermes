using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;
public interface IServerDataRepository
{
    public Task AddServerData(ServerData serverData);
    public Task<IEnumerable<ServerData>> GetServerData();
    public Task DeleteExpired();
}
