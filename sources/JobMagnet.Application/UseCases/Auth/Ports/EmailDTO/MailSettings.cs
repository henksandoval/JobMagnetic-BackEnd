namespace JobMagnet.Application.UseCases.Auth.Ports.EmailDTO;
public class MailSettings
{
    public string Host { get; init; }
    public int Port { get; init; }
    public string SenderName { get; init; }
    public string SenderEmail { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
}