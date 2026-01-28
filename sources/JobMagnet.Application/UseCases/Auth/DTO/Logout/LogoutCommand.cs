using CommunityToolkit.Diagnostics;

namespace JobMagnet.Application.UseCases.Auth.DTO.Logout;
public class LogoutCommand
{
    public string RefreshToken { get; set; }
    public Guid UserId { get; set; }
    
    public LogoutCommand(string refreshToken, Guid userId)
    {
        Guard.IsNotNullOrWhiteSpace(refreshToken);

        RefreshToken = refreshToken;
        UserId = userId;
    }
}