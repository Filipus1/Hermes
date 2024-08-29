using Hermes.Application.Abstraction;
using Hermes.Application.Entities;

namespace Hermes.Application.Services;
public class TokenService : ITokenService
{
    private readonly ITokenRepository _repository;

    public TokenService(ITokenRepository repository)
    {
        _repository = repository;
    }

    public async Task<InvitationToken?> Create(string email)
    {
        return await _repository.CreateToken(email);
    }

    public async Task<InvitationToken?> Get(string token)
    {
        return await _repository.GetToken(token);
    }

    public async Task Use(string token)
    {
        await _repository.UseToken(token);
    }

    public async Task<bool> Validate(string token)
    {
        return await _repository.IsTokenValid(token);
    }
}
