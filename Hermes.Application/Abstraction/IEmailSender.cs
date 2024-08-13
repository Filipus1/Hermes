namespace Hermes.Application.Abstraction;
public interface IEmailRepository
{
    public Task<bool> SendEmail(string receiverEmail, string body);
}