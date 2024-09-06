using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;

public interface IServerDataRepository
{
    public Task<bool> AddServerData(ServerData serverData);
    public Task<IEnumerable<ServerData>> GetServerData();
}
