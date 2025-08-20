using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;

namespace JobMagnet.Application.UseCases.Auth;

public class AuthUserHandler(IUserManagerAdapter userManagerAdapter) : IAuthUserHandler
{
    public async Task<UserToken> LoginAsync(LoginDto loginDto)
    {
        if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            throw new ArgumentException("Email and password are required.");

        return await userManagerAdapter.LoginAsync(loginDto);
    }
}