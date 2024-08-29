using Hermes.Application.Abstraction;

namespace Hermes.Application.Services;
public class EmailService : IEmailService
{
    private readonly IEmailSender _sender;

    public EmailService(IEmailSender sender)
    {
        _sender = sender;
    }

    public async Task<bool> Send(string receiverEmail, string body)
    {
        return await _sender.SendEmail(receiverEmail, body);
    }
}