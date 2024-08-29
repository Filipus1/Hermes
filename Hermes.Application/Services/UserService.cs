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

    public async Task<bool> Create(User user)
    {
        return await _userRepository.CreateUser(user);
    }

    public async Task<bool> Delete(List<User> usersToDelete)
    {
        return await _userRepository.DeleteUsers(usersToDelete);
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
}