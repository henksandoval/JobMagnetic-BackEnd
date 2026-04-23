using CommunityToolkit.Diagnostics;

namespace JobMagnet.Application.UseCases.Auth.DTO.ForgotPassword;

public class  ResetTokenDto
{
    public string ResetToken { get; set; }
    public Guid UserId { get; set; }
    
    public ResetTokenDto(string resetToken, Guid userId)
    {
        Guard.IsNotNullOrWhiteSpace(resetToken);

        ResetToken = resetToken;
        UserId = userId;
    }
}