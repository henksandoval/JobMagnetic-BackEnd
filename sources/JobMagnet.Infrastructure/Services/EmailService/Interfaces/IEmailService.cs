using JobMagnet.Application.UseCases.Auth.Ports.EmailDTO;

namespace JobMagnet.Infrastructure.Services.EmailService.Interfaces;
public interface IEmailService
{
    Task SendEmailAsync(MailCommand  mailCommand);
}