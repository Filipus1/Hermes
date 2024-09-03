using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Infrastructure.Context;
using Hermes.Infrastructure.TokenGenerator;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Repositories;
public class TokenRepository : ITokenRepository
{
    private readonly AppDbContext _context;
    private readonly ITokenGenerator _generator;

    public TokenRepository(AppDbContext context, ITokenGenerator generator)
    {
        _context = context;
        _generator = generator;
    }

    public async Task<InvitationToken?> CreateToken(string email)
    {

        var token = _generator.GenerateToken();

        var invitationToken = new InvitationToken
        {
            CreatedBy = email,
            Token = token,
            ExpiryDate = DateTime.UtcNow.AddMinutes(15)
        };

        await _context.AddAsync(invitationToken);
        await _context.SaveChangesAsync();

        return invitationToken;

    }

    public async Task<InvitationToken?> GetToken(string token)
    {
        return await _context.InvitationTokens.SingleOrDefaultAsync(it => it.Token == token);
    }

    public async Task MarkTokenAsUsed(string token)
    {
        var searchedToken = await GetToken(token);

        if (searchedToken == null) return;

        searchedToken.IsUsed = true;
        await _context.SaveChangesAsync();
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
