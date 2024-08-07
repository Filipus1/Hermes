using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers();
    Task<User?> GetUserByGuid(Guid userGuid);
    Task<User?> GetUserByCredentials(string email, string password);
    Task<User?> GetUserByEmail(string email);
    Task<bool> CreateUser(User user);
    Task<bool> DeleteUsers(List<User> usersToDelete);
    Task<bool> UpdateUser(User user);
}