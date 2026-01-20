using CommunityToolkit.Diagnostics;

namespace JobMagnet.Application.UseCases.Auth.DTO;

public class ForgotPasswordCommand
{
    public string Email { get; set; }

    public ForgotPasswordCommand(string email)
    {
        Guard.IsNotNullOrWhiteSpace(email);
        Email = email;
    }
}