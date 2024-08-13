using Hermes.Application.Abstraction;
using Hermes.Application.Entities;

namespace Hermes.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository userRepository;

    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<bool> Create(User user)
    {
        return await userRepository.CreateUser(user);
    }

    public async Task<bool> Delete(List<User> usersToDelete)
    {
        return await userRepository.DeleteUsers(usersToDelete);
    }

    public async Task<User?> Get(Guid userGuid)
    {
        return await userRepository.GetUserByGuid(userGuid);
    }

    public async Task<User?> Get(string email, string password)
    {
        return await userRepository.GetUserByCredentials(email, password);
    }

    public async Task<User?> Get(string email)
    {
        return await userRepository.GetUserByEmail(email);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await userRepository.GetUsers();
    }
}