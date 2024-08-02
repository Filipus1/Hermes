using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers();
    Task<User?> GetUserByID(int userId);
    Task<User?> GetUserByCredentials(string email, string password);
    Task<User?> GetUserByEmail(string email);
    Task<bool> CreateUser(User user);
    Task<bool> DeleteUser(int userID);
    Task<bool> UpdateUser(User user);
}