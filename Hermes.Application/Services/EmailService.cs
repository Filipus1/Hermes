using Hermes.Application.Abstraction;

public class EmailService : IEmailService
{
    private IEmailSender sender;

    public EmailService(IEmailSender sender)
    {
        this.sender = sender;
    }

    public async Task<bool> Send(string receiverEmail, string body)
    {
        return await sender.SendEmail(receiverEmail, body);
    }
}