namespace Hermes.Application.Abstraction;
public interface IEmailService
{
    public Task<bool> Send(string receiverEmail, string body);
}