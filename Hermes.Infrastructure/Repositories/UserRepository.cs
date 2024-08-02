using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppContext context;
    private readonly IPasswordHasher<User> passwordHasher;

    public UserRepository(AppContext context, IPasswordHasher<User> passwordHasher)
    {
        this.context = context;
        this.passwordHasher = passwordHasher;
    }

    public async Task<bool> CreateUser(User user)
    {
        await context.Users.AddAsync(user);
        user.Password = passwordHasher.HashPassword(user, user.Password);

        return await context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> DeleteUser(int UserId)
    {
        var userToDelete = await context.Users.FirstOrDefaultAsync(user => user.Id == UserId);

        return userToDelete != null;
    }

    public async Task<User?> GetUserByCredentials(string email, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Email == email);

        if (user == null) return null;

        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, password);

        return verificationResult == PasswordVerificationResult.Success ? user : null;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<User?> GetUserByID(int userId)
    {
        return await context.Users.FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<bool> UpdateUser(User user)
    {
        context.Users.Update(user);

        return await context.SaveChangesAsync() >= 1;
    }
}