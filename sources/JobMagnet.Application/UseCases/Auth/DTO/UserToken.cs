namespace JobMagnet.Application.UseCases.Auth.DTO;

public class UserToken
{
    public string Token { get; set; }
    public DateTime Expiracion { get; set; }
}