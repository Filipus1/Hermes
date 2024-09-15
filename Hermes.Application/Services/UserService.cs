using Hermes.Application.Abstraction;
using Hermes.Application.Entities;

namespace Hermes.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task Create(User user)
    {
        await _repository.CreateUser(user);
    }

    public async Task Delete(List<User> usersToDelete)
    {
        await _repository.DeleteUsers(usersToDelete);
    }

    public async Task<User?> Get(Guid userGuid)
    {
        return await _repository.GetUserByGuid(userGuid);
    }

    public async Task<User?> Get(string email, string password)
    {
        return await _repository.GetUserByCredentials(email, password);
    }

    public async Task<User?> Get(string email)
    {
        return await _repository.GetUserByEmail(email);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _repository.GetUsers();
    }

    public async Task<IEnumerable<User>> GetCollaborators()
    {
        var users = await GetAll();
        var collaborators = users
            .Where(u => u.Role == "collaborator")
            .ToList();

        return collaborators;
    }
}