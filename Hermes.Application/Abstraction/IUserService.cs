using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;

public interface IUserService
{
    public Task<bool> Create(User user);
    public Task<bool> Delete(int userId);
    public Task<User?> Get(int userId);
    public Task<User?> Get(string email, string password);
    public Task<User?> Get(string email);
    public Task<IEnumerable<User>> GetAll();
}