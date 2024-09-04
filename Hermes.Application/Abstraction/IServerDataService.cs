using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;

public interface IServerDataService
{
    public Task<bool> Add(ServerData serverData);
    public Task<IEnumerable<ServerData>> Get();
}
