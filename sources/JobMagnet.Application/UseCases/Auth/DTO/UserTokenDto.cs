namespace JobMagnet.Application.UseCases.Auth.DTO;

public class UserTokenDto
{
    public string Token { get; set; } 
    public DateTime Expiration { get; set; }
    public string RefreshToken { get; set; } 
}