using CommunityToolkit.Diagnostics;

namespace JobMagnet.Application.UseCases.Auth.DTO.ForgotPassword;

public class ForgotPasswordCommand
{
    public string Email { get; }

    public ForgotPasswordCommand(string email)
    {
        Guard.IsNotNullOrWhiteSpace(email);
        
        Email = email;
    }
}