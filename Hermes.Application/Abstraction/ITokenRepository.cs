using Hermes.Application.Entities;

namespace Hermes.Application.Abstraction;
public interface ITokenRepository
{
    public Task<InvitationToken?> CreateToken(string email);
    public Task<InvitationToken?> GetToken(string token);
    public Task<bool> IsTokenValid(string token);
    public Task MarkTokenAsUsed(string token);
}
