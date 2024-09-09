using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;
public interface IUserService
{
    public Task Create(User user);
    public Task Delete(List<User> usersToDelete);
    public Task<User?> Get(Guid userGuid);
    public Task<User?> Get(string email, string password);
    public Task<User?> Get(string email);
    public Task<IEnumerable<User>> GetCollaborators();
    public Task<IEnumerable<User>> GetAll();
}