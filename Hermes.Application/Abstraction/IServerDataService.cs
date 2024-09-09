using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;

public interface IServerDataService
{
    public Task Add(ServerData serverData);
    public Task<IEnumerable<ServerData>> Get();
    public Task DeleteExpired();
}
