using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Infrastructure.Context;
using Hermes.Infrastructure.TokenGenerator;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Repositories;
public class TokenRepository : ITokenRepository
{
    private readonly AppDbContext context;
    private readonly ITokenGenerator generator;

    public TokenRepository(AppDbContext context, ITokenGenerator generator)
    {
        this.context = context;
        this.generator = generator;
    }

    public async Task<InvitationToken?> CreateToken(string email)
    {
        try
        {
            var token = generator.GenerateToken();

            var invitationToken = new InvitationToken
            {
                CreatedBy = email,
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddMinutes(15)
            };

            await context.AddAsync(invitationToken);
            await context.SaveChangesAsync();

            return invitationToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            throw;
        }
    }

    public async Task<InvitationToken?> GetToken(string token)
    {
        return await context.InvitationTokens.SingleOrDefaultAsync(it => it.Token == token);
    }

    public async Task UseToken(string token)
    {
        var searchedToken = await GetToken(token);

        if (searchedToken == null) return;

        searchedToken.IsUsed = true;
        await context.SaveChangesAsync();
    }

    public async Task<bool> IsTokenValid(string token)
    {
        var invitationToken = await GetToken(token);

        if (invitationToken == null || invitationToken.IsUsed || DateTime.UtcNow >= invitationToken.ExpiryDate)
        {
            return false;
        }

        return true;
    }
}
