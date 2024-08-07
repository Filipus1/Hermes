namespace Hermes.Application;

public interface ITokenService
{
    public Task<InvitationToken?> Create(string email);
    public Task<InvitationToken?> Get(string token);
    public Task<bool> Validate(string token);
    public Task Use(string token);
}
