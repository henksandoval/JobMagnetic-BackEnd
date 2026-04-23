namespace JobMagnet.Application.UseCases.Auth.DTO.GoogleLogin;

public class GoogleTokenInfoDto
{
    public string AccessToken { get; set; }
    public DateTime ExpiresInSeconds { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    
    // Opcionales muy recomendados:
    public string UserId { get; set; }
    public string PictureUrl { get; set; }
    public string Role { get; set; } 
}