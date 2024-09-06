using Hermes.Application.Abstraction;
using Hermes.Infrastructure.Config;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Hermes.Infrastructure.Email;
public class EmailSender : IEmailSender
{
    private readonly MimeMessage _message;
    private readonly IEmailConfig _emailConfig;

    public EmailSender(MimeMessage message, IEmailConfig emailConfig)
    {
        _message = message;
        _emailConfig = emailConfig;
    }

    public async Task<bool> SendEmail(string receiverEmail, string topic, string body)
    {
        try
        {
            _message.From.Add(MailboxAddress.Parse(_emailConfig.Username));
            _message.To.Add(MailboxAddress.Parse(receiverEmail));
            _message.Subject = topic;
            _message.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
            await smtp.SendAsync(_message);
            await smtp.DisconnectAsync(true);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while sending email: {ex.Message}");

            return false;
        }
    }
}