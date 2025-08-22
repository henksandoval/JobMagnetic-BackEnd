using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;

namespace JobMagnet.Application.UseCases.Auth;

public class AuthUserHandler(IUserManagerAdapter userManagerAdapter) : IAuthUserHandler
{
    public async Task<UserToken> LoginAsync(LoginDto loginDto)
    {
        if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            throw new ArgumentException("The email and password cannot be null, empty, or contain only spaces.");
        
        var token = await userManagerAdapter.LoginAsync(loginDto);
        return false ? null : token;
    }
}