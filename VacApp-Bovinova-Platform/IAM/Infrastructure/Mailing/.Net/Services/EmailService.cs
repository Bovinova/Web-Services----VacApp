using System.Net;
using System.Net.Mail;

namespace VacApp_Bovinova_Platform.IAM.Infrastructure.Mailing.Net.Services;


public class EmailService
{
    private readonly string smtpServer;
    private readonly int port;
    private readonly string fromEmail;
    private readonly string password;

    public EmailService(string smtpServer, int port, string fromEmail, string password)
    {
        this.smtpServer = smtpServer;
        this.port = port;
        this.fromEmail = fromEmail;
        this.password = password;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using var smtpClient = new SmtpClient(smtpServer, port)
        {
            Credentials = new NetworkCredential(fromEmail, password),
            EnableSsl = true // Header for secure connection
        };

        var mailMessage = new MailMessage(fromEmail, toEmail, subject, body)
        {
            IsBodyHtml = true // If the body contains HTML
        };

        await smtpClient.SendMailAsync(mailMessage);
    }
}