namespace Hermes.Infrastructure.Config;

public class EmailConfig : IEmailConfig
{
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public EmailConfig()
    {
        SmtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER")!;
        Port = int.Parse(Environment.GetEnvironmentVariable("PORT")!);
        Username = Environment.GetEnvironmentVariable("EMAIL_SENDER_USERNAME")!;
        Password = Environment.GetEnvironmentVariable("EMAIL_SENDER_PASSWORD")!;
    }
}

public interface IEmailConfig
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}