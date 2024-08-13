using Hermes.Application.Abstraction;
using Hermes.Application.Entities;

namespace Hermes.Application.Services;
public class TokenService : ITokenService
{
    private readonly ITokenRepository repository;

    public TokenService(ITokenRepository repository)
    {
        this.repository = repository;
    }

    public async Task<InvitationToken?> Create(string email)
    {
        return await repository.CreateToken(email);
    }

    public async Task<InvitationToken?> Get(string token)
    {
        return await repository.GetToken(token);
    }

    public async Task Use(string token)
    {
        await repository.UseToken(token);
    }

    public async Task<bool> Validate(string token)
    {
        return await repository.IsTokenValid(token);
    }
}
