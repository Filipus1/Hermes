namespace Hermes.Application;

public interface ITokenRepository
{
    public Task<InvitationToken?> CreateToken(string email);
    public Task<InvitationToken?> GetToken(string token);
    public Task<bool> IsTokenValid(string token);
    public Task UseToken(string token);
}
