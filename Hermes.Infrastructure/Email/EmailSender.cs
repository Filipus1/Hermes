using Hermes.Application.Abstraction;
using Hermes.Infrastracture.Config;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Hermes.Infrastructure.Email;
public class EmailSender : IEmailSender
{

    private readonly MimeMessage message;
    private readonly IEmailConfig emailConfig;

    public EmailSender(MimeMessage message, IEmailConfig emailConfig)
    {
        this.message = message;
        this.emailConfig = emailConfig;
    }

    public async Task<bool> SendEmail(string receiverEmail, string body)
    {
        message.From.Add(MailboxAddress.Parse(emailConfig.Username));
        message.To.Add(MailboxAddress.Parse(receiverEmail));
        message.Subject = "Invitation";
        message.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();

        try
        {
            await smtp.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(emailConfig.Username, emailConfig.Password);
            await smtp.SendAsync(message);
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