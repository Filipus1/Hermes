using Hermes.Application.Abstraction;
using Hermes.Application.Entities;

namespace Hermes.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Create(User user)
    {
        await _userRepository.CreateUser(user);
    }

    public async Task Delete(List<User> usersToDelete)
    {
        await _userRepository.DeleteUsers(usersToDelete);
    }

    public async Task<User?> Get(Guid userGuid)
    {
        return await _userRepository.GetUserByGuid(userGuid);
    }

    public async Task<User?> Get(string email, string password)
    {
        return await _userRepository.GetUserByCredentials(email, password);
    }

    public async Task<User?> Get(string email)
    {
        return await _userRepository.GetUserByEmail(email);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userRepository.GetUsers();
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