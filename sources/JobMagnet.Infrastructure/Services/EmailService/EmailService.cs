using JobMagnet.Application.UseCases.Auth.Ports.EmailDTO;
using JobMagnet.Infrastructure.Services.EmailService.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace JobMagnet.Infrastructure.Services.EmailService;
public class EmailService(IOptions<MailSettings> mailSettings) : IEmailService
{
    private readonly MailSettings _mailSettings = mailSettings.Value;

    public async Task SendEmailAsync(MailCommand mailCommand)
    {
        var emailMessage = new MimeMessage();
        emailMessage.Sender = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
        emailMessage.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
        
        emailMessage.To.Add(MailboxAddress.Parse(mailCommand.ToEmail));
        emailMessage.Subject = mailCommand.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = mailCommand.Body
        };
        
        emailMessage.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
        await smtp.SendAsync(emailMessage);
        await smtp.DisconnectAsync(true);
    }
}