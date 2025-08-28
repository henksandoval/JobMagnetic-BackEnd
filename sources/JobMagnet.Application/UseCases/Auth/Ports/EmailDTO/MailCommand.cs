namespace JobMagnet.Application.UseCases.Auth.Ports.EmailDTO;

public class MailCommand
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}