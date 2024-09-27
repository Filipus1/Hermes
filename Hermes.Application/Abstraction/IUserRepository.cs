using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers();
    Task<User?> GetUserByGuid(Guid userGuid);
    Task<User?> GetUserByCredentials(string email, string password);
    Task<User?> GetUserByEmail(string email);
    Task CreateUser(User user);
    Task DeleteUsers(List<User> usersToDelete);
    Task UpdateUser(User user);
    Task<bool> IsEmailUnique(string email);
}