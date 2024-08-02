using Hermes.Application.Abstraction;

public class EmailService : IEmailService
{
    private IEmailRepository emailRepository;

    public EmailService(IEmailRepository emailRepository)
    {
        this.emailRepository = emailRepository;
    }

    public async Task<bool> Send(string receiverEmail, string body)
    {
        return await emailRepository.SendEmail(receiverEmail, body);
    }
}