namespace JobMagnet.Application.UseCases.Auth.DTO;

public class UserTokenDto
{
    public string AccessToken { get; set; } 
    public DateTime ExpiresInSeconds { get; set; }
    public string? RefreshToken { get; set; } 
}