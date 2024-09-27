using Hermes.Application.Entities;
using Hermes.Application.Entities.Dto;

namespace Hermes.Application.Abstraction;
public interface IUserService
{
    public Task Create(User user);
    public Task Delete(List<User> usersToDelete);
    public Task<User?> Get(Guid userGuid);
    public Task<User?> Get(string email, string password);
    public Task<User?> Get(string email);
    public Task<IEnumerable<CollaboratorDto>> GetCollaborators();
    public Task<IEnumerable<User>> GetAll();
}