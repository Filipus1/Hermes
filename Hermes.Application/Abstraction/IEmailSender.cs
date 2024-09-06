namespace Hermes.Application.Abstraction;
public interface IEmailSender
{
    public Task<bool> SendEmail(string receiverEmail, string topic, string body);
}