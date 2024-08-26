using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserRepository(AppDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        user.Password = _passwordHasher.HashPassword(user, user.Password);

        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> DeleteUsers(List<User> usersToDelete)
    {
        if (usersToDelete == null) return false;

        foreach (var user in usersToDelete)
        {
            _context.Users.Remove(user);
        }

        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<User?> GetUserByCredentials(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);

        if (user == null) return null;

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

        return verificationResult == PasswordVerificationResult.Success ? user : null;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<User?> GetUserByGuid(Guid userGuid)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Guid == userGuid);
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<bool> UpdateUser(User user)
    {
        _context.Users.Update(user);

        return await _context.SaveChangesAsync() >= 1;
    }
}